using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class GemView : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    public Gem gem = new Gem();

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void InitializeWithGemInfo(GemInfo gemInfo)
    {
        _image.sprite = gemInfo.sprite;
    }

    public void MoveTo(Vector2 coordinates)
    {
        _rectTransform.position = coordinates;
    }
}
