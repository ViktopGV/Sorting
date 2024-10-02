using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LevelSelector : MonoBehaviour
{
    public Button[] LevelButtons => _buttons;

    [SerializeField] private GameController _gameController;
    private Button[] _buttons;

    private void Awake()
    {
        _buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].interactable = true;

            Text text = _buttons[i].GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = (i + 1).ToString();
                if (YandexGame.savesData.OpenedLevel < i)
                    _buttons[i].interactable = false;
            }
            else continue;

            int index = i;

            _buttons[i].onClick.AddListener(() => {
                YandexGame.savesData.CurrentLevel = index;
                _gameController.LoadCurrentLevel();
                gameObject.SetActive(false);
            });
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].interactable = true;

            Text text = _buttons[i].GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = (i + 1).ToString();
                if (YandexGame.savesData.OpenedLevel < i)
                    _buttons[i].interactable = false;
            }
            else continue;

        }
    }

}
