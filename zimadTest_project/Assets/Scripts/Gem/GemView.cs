using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemView : MonoBehaviour
{
    private Image _image;
    

    public void InitializeWithGemInfo(GemInfo gemInfo)
    {
        _image.sprite = gemInfo.sprite;
    }

    public void MoveTo(Vector2 coordinates)
    {
        transform.localPosition = coordinates;
    }
}
