using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Animator))]  

public class PausePanel : Panel
{
    //public UnityAction RestartButtonClicked;
    //public UnityAction MainMenuButtonCLicked;

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
    }
}
