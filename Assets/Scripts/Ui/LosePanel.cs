using UnityEngine;

public class LosePanel : Panel
{
    [SerializeField] private AudioSource _loseSound;

    public void PlayLoseSound()
    {
        _loseSound.Play();
    }
}
