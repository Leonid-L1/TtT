using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    protected static string ToScreen = "PanelToScreen";
    protected static string FromScreen = "PanelFromScreen";

    private Animator _animator;

    public bool IsOnScreen { get; private set; } = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void MoveToScreen()
    {
        _animator.Play(ToScreen);
        IsOnScreen = true;
    }

    public void MoveFromScreen()
    {
        _animator.Play(FromScreen);
        IsOnScreen = false;
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}
