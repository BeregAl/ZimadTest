using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemChain
{
    public List<Gem> chainedGems = new List<Gem>();

    public GemChain(List<Gem> _chainedGems)
    {
        chainedGems = _chainedGems;
    }
}
