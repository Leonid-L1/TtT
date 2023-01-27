using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToStunTransition : Transition
{
    public void SetAsReady()
    {
        IsReadyToTransit = true;
    }
}
