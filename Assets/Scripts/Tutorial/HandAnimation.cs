using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandAnimation : MonoBehaviour
{
    [SerializeField] private Image _handImage;

    private void Start()
    {
        Tween.PunchScale(_handImage.transform, Vector3.one * .1f, 5, cycles: 100, frequency: 3);
    }
}
