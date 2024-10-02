using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameController : MonoBehaviour
{
    [SerializeField] private LevelSelector _selector;
    [SerializeField] private int _level;
    [SerializeField] private Button _yesSkipBtn;
    [SerializeField] private AditionalPostController _addPost;

    private const string levelFileName = "Level_";
    private Level _currentLevel;
    private int _filledPostsCount = 0;

    private void Start()
    {
        LoadCurrentLevel();
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
        _yesSkipBtn.onClick.AddListener(() => YandexGame.RewVideoShow(0));
    }

    private void Rewarded(int id)
    {
        if(id == 0)//Skip level
        {
            if (YandexGame.savesData.OpenedLevel == YandexGame.savesData.CurrentLevel)
                YandexGame.savesData.OpenedLevel++;
            YandexGame.savesData.CurrentLevel++;
            YandexGame.SaveProgress();
            LoadCurrentLevel();
        }
        else
        {
            _currentLevel.AddPostBtn.gameObject.SetActive(false);
            _currentLevel.AdditionalPost.gameObject.SetActive(true);
        }
    }

    public void LoadCurrentLevel(int wait = 0)
    {
        StartCoroutine( LoadLevelById(YandexGame.savesData.CurrentLevel, wait));        
    }

    public IEnumerator LoadLevelById(int id, int wait = 0)
    {
        if(_currentLevel != null)
        {
            yield return StartCoroutine(DestroyLevel(wait));
            YandexGame.FullscreenShow();
        }
        if (id > 50) id = 3;
        _filledPostsCount = 0;
        YandexGame.savesData.CurrentLevel = id;
        YandexGame.SaveProgress();
        string levelPath = levelFileName + id;
        _currentLevel = Instantiate(Resources.Load<Level>(levelPath));
        foreach (var post in _currentLevel.Posts)
        {
            post.PostFilled += CheckLevelComplete;
        }
        if(_currentLevel.AddPostBtn != null)
            _currentLevel.AddPostBtn.onClick.AddListener(ShowAddPostText);
    }

    private void ShowRewardPost()
    {
        YandexGame.RewVideoShow(1);
        _addPost.Yes.onClick.RemoveListener(ShowRewardPost);

    }

    private void ShowAddPostText()
    {
        _addPost.gameObject.SetActive(true);
        _addPost.Yes.onClick.AddListener(ShowRewardPost);  
    }

    public void CheckLevelComplete(Post obj)
    {
        obj.PlaySparks();
        _filledPostsCount++;
        if (_filledPostsCount == _currentLevel.ColorsCount())
        {
            if (YandexGame.savesData.OpenedLevel == YandexGame.savesData.CurrentLevel)
                YandexGame.savesData.OpenedLevel++;
            YandexGame.savesData.CurrentLevel += 1;
            
            YandexGame.SaveProgress();

            foreach (var post in _currentLevel.Posts)
            {
                if(post.DonutsCount != 0)
                    post.PlayConfetti();
            }
            LoadCurrentLevel(2);
        }
    }

    public void ReloadLevel() => LoadCurrentLevel();


    public IEnumerator DestroyLevel(float waitTime)
    {
        _filledPostsCount = 0;
        yield return new WaitForSeconds(waitTime);
        foreach (var post in _currentLevel.Posts)
        {
            post.PostFilled -= CheckLevelComplete;
        }
        if (_currentLevel.AddPostBtn != null)
            _currentLevel.AddPostBtn.onClick.RemoveListener(ShowAddPostText);
        Destroy(_currentLevel.gameObject);
        _currentLevel = null;
    }

    public void ActivateAdditionalPost()
    {
        _currentLevel.AdditionalPost.gameObject.SetActive(true);
        _currentLevel.AddPostBtn.gameObject.SetActive(false);
    }

}
