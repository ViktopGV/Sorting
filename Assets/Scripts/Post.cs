using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Post : MonoBehaviour
{
    public event Action<Post> PostFilled;
    public int DonutsCount => _donuts.Count;
    [SerializeField] private List<Donut> _donuts;
    [SerializeField] private float _bottomDonutYPos;
    [SerializeField] private float _donutOffset;
    [SerializeField] private int _maxDonutsCount = 4;
    [SerializeField] private ParticleSystem _confetti;
    [SerializeField] private ParticleSystem _sparks;

    public void AddDonut(Donut donut)
    {
        donut.transform.SetParent(transform);
        _donuts.Add(donut);
        if(CheckColorFilled())
        {
            PostFilled?.Invoke(this);
        }                    
    }

    private IEnumerator ParticleWaiter()
    {
        yield return new WaitForSeconds(.7f);
        PlayConfetti();
        PlaySparks();
    }

    public void PlayConfetti() => _confetti.Play();
    public void PlaySparks() => _sparks.Play();

    public void OpenTopDonutColor()
    {
        Donut donut = GetTopDonut();
        if (donut != null)        
            donut.OpenColor();
    }

    public void InitializePost(int maxDonuts, Donut[] donuts)
    {
        if (donuts.Length > maxDonuts)
            throw new System.Exception("Array length is more then max count");

        _maxDonutsCount = maxDonuts;
        foreach(var donut in donuts)
        {
            AddDonut(donut);
        }
        InitializeDonutPosition();
    }

    public Donut PopDonut()
    {
        if (_donuts.Count == 0 || CheckColorFilled() == true) return null;
        Donut topDonut = GetTopDonut();
        _donuts.Remove(topDonut);
        return topDonut;
    }

    public Donut GetTopDonut() => _donuts.Count == 0 ? null : _donuts[_donuts.Count - 1];

    public float GetDonutYPosition()
    {
        return transform.position.y + _bottomDonutYPos + (_donutOffset * _donuts.Count);
    }

    public bool CanAddDonut(Donut donut)
    {        
        if(_donuts.Count >= _maxDonutsCount) return false;
        if(_donuts.Count > 0)
        {
            if (donut.transform.parent == GetTopDonut().transform.parent) return true;
            if (GetTopDonut().DonutColor != donut.DonutColor) return false;
        }
        return true;
    }

    public bool HaveDonuts() => _donuts.Count > 0;

    [ContextMenu("Init Post")]
    public void InitializeDonutPosition()
    {
        _donuts = new List<Donut>(GetComponentsInChildren<Donut>());
        for (int i = 0; i < _donuts.Count; ++i)
        {
            float yPos = _bottomDonutYPos + (_donutOffset * i);
            _donuts[i].transform.localPosition = new Vector3(0, yPos, 0);
        }        
    }

    private bool CheckColorFilled()
    {
        if (_donuts.Count == _maxDonutsCount)
        {
            Colors checkColor = GetTopDonut().DonutColor;
            foreach (var donut in _donuts)
            {
                if (donut.DonutColor != checkColor)
                {
                    return false;
                }
                donut.OpenColor();
            }
            return true;
        }
        return false;
    }

    private void OnValidate()
    {
        InitializeDonutPosition();        
    }
}
