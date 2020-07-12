using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class GemView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image _image;

    public Gem gem = new Gem();

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        gem.onSlotChanged += OnSlotChanged;
        gem.gemView = this;
    }

    private void OnDestroy()
    {
        gem.onSlotChanged -= OnSlotChanged;
    }

    private void OnSlotChanged(Slot obj)
    {
        
    }

    public void InitializeWithGemInfo(GemInfo gemInfo)
    {
        _image.sprite = gemInfo.sprite;
    }

    public void MoveTo(Vector2 coordinates, bool withAnimation = false)
    {
        if (withAnimation)
        {
            _rectTransform.DOMove(coordinates, 0.3f);
        }
        else
        {
            _rectTransform.position = coordinates;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerControl.instance.ClickOnGem(gem);
    }

    public void ShakeGem()
    {
        transform.DOShakeScale(0.3f, 0.1f);
    }
}
