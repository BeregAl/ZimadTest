using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GemsPool 
{
    
    public static Queue<GemView> pool = new Queue<GemView>();

    public static GemView GetFromPool()
    {
        return pool.Dequeue();
    }

    public static void ReturnToPool(GemView gemToReturn)
    {
        gemToReturn.gameObject.SetActive(true);
        pool.Enqueue(gemToReturn);
    }
}
