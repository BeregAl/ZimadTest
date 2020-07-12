using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Create Gem Info", fileName = "GemInfo")]
public class GemInfo : ScriptableObject
{
    public int id;
    public Sprite sprite;
    public GemColor gemColor;
    public GemType gemType;
}