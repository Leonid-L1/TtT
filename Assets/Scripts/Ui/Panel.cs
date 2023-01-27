using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    private static string ToScreen = "PanelToScreen";
    private static string FromScreen = "PanelFromScreen";

    //private Animator _animator;

    public bool IsOnScreen { get; private set; } = false;

    //private void Awake()
    //{
    //    _animator = GetComponent<Animator>();
    //}

    public virtual void MoveToScreen()
    {
        GetComponent<Animator>().Play(ToScreen);
        //_animator.Play(ToScreen);
        IsOnScreen = true;
    }

    public virtual void MoveFromScreen()
    {
        GetComponent<Animator>().Play(FromScreen);
        //_animator.Play(FromScreen);
        IsOnScreen = false;
    }
}
