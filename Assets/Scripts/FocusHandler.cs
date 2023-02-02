using UnityEngine;

public class FocusHandler : MonoBehaviour
{
    private void OnApplicationFocus(bool focus)
    {
        if(focus == true)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
}
