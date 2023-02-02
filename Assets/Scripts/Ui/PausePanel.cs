using UnityEngine;

public class PausePanel : Panel
{
    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
    }
}
