using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Create Gem Info", fileName = "GemInfo")]
public class GemInfo : ScriptableObject
{
    public int id;
    public Sprite sprite;
    public GemType gemType;
}

public enum GemType
{
    None=0,
    Red=1,
    Green=2,
    Blue=3,

    Red_Charged=21,
    Green_Charged=22,
    Blue_Charged=23
}
