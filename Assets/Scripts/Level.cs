using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public int ColorsCount()
    {
        int donuts = 0;
        foreach(var post in _posts)
        {
            donuts += post.DonutsCount;
        }
        return donuts / 4;
    }
    public Post[] Posts => _posts;
    public Button AddPostBtn => _addPostButton;
    public Post AdditionalPost => _additionalPost;


    [SerializeField] private Post[] _posts;
    [SerializeField] private Button _addPostButton;
    [SerializeField] private Post _additionalPost;
    [SerializeField] private Canvas _canvas;

    [ContextMenu("Init")]
    public void Init()
    {
        _posts = GetComponentsInChildren<Post>();
    }

    private void Awake()
    {
        if(_canvas != null)
            _canvas.worldCamera = Camera.main;
    }
}
