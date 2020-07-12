using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemChain
{
    public List<Gem> chainedChems = new List<Gem>();

    public GemChain(List<Gem> _chainedChems)
    {
        chainedChems = _chainedChems;
    }
}
