using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemsLibrary : MonoBehaviour
{
    public static List<GemInfo> gems = new List<GemInfo>();

    public static List<GemInfo> CommonGems => gems.Where(x => x.gemType == GemType.Regular).ToList();

    private void Awake()
    {
        gems = Resources.LoadAll<GemInfo>("GemInfo").ToList<GemInfo>();
        Debug.Log($"Library contains {gems.Count} gems");
    }

}
