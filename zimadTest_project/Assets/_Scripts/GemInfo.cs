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

public enum GemType
{
    None=0,
    Regular=1,
    Charged=2,
}

public enum GemColor
{
    None=0,
    Red=1,
    Green=2,
    Blue=3,
}
