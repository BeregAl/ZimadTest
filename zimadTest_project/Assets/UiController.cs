using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public RectTransform gravityImageRectTransform;


    private void Awake()
    {
        PlayerControl.instance.onGravityDirectionChange += OnGravityDirectionChange;
    }

    private void OnGravityDirectionChange(GravityDirection gd)
    {
        if (gd == GravityDirection.Up)
        {
            gravityImageRectTransform.DORotate(Vector3.zero, 0.1f);
        }
        else if (gd == GravityDirection.Down)
        {
            gravityImageRectTransform.DORotate(Vector3.back*180f, 0.1f);
        }
    }

    private void OnDestroy()
    {
        PlayerControl.instance.onGravityDirectionChange -= OnGravityDirectionChange;
    }
}
