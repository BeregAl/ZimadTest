using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourChargedGem : IGemBehaviour
{
    public void DestroyOnMatch()
    {
        PlayerControl.gravityChangePending = true;
    }
}
