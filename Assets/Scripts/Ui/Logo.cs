using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(Animator))]
public class Logo : MonoBehaviour
{
    private float _delay = 2;

    public void OnClick()
    {
        Destroy(gameObject, _delay);
    }

}
