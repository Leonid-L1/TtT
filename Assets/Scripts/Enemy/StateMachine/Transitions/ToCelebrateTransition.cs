using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToCelebrateTransition : Transition
{
    public void SetReady()
    {
        IsReadyToTransit = true;
    }
}
