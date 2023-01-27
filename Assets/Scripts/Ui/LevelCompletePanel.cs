using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletePanel : Panel
{
    [SerializeField] private AudioSource _winSound;
    [SerializeField] private LevelHandler _levelHandler;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private List<Animation> _stars;
    [SerializeField] private float _moveToScreenDuration;

    private float _puaseBetweenStarsScaling = 0.5f;
    private float _timeBeforeDisable = 3;

    private void OnEnable()
    {
        foreach (Button button in _buttons)
            button.interactable = false;      
    }

    public override void MoveToScreen()
    {
        base.MoveToScreen();
        _winSound.Play();
    }
    public override void MoveFromScreen()
    {
        base.MoveFromScreen();     
        StartCoroutine(DisablePanel());
    }

    public void SetStars(int stasrCount)
    {
        StartCoroutine(AnimateStars(stasrCount));
    }

    private IEnumerator AnimateStars(int starsCount)
    {             
        yield return new WaitForSeconds(_moveToScreenDuration);

        for (int i = 0; i < starsCount; i++)
        {   
            _stars[i].Play();
            yield return new WaitForSeconds(_puaseBetweenStarsScaling);
        }
        foreach (Button button in _buttons)
            button.interactable = true;

        yield break;
    }

    private IEnumerator DisablePanel()
    {
        yield return new WaitForSecondsRealtime(_timeBeforeDisable);

        foreach (var star in _stars)
        {
            star.Rewind();
            star.Play();
            star.Sample();
            star.Stop();
        }
        gameObject.SetActive(false);
        yield break;
    }
}
