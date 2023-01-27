using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicHandler : MonoBehaviour
{   
    [SerializeField] private List<AudioSource> _audioSources;

    private AudioSource _currentMusic;
    private float _maxVolume = 1;
    private float _minVolume = 0;
    private float _volumeChangeSpeed = 0.5f;

    private enum Music
    {
        menu = 0,
        runnerPhaze = 1,
        actionPhaze = 2
    }

    private void Start()
    {
        foreach(var audioSource in _audioSources)
            audioSource.volume = _minVolume;

        _currentMusic = _audioSources[(int)Music.menu];
        _currentMusic.Play();
        _currentMusic.volume = 1;
    }

    public void SetToRunnerPhaze()
    {   
        if(_currentMusic != _audioSources[(int)Music.runnerPhaze])
            StartCoroutine(ChangeMusic(_audioSources[(int)Music.runnerPhaze]));
    }

    public void SetToActionPhaze()
    {
        StartCoroutine(ChangeMusic(_audioSources[(int)Music.actionPhaze]));
    }
    
    public void SetToMainMenu()
    {
        StartCoroutine(ChangeMusic(_audioSources[(int)Music.menu]));
    }

    public void StopMusic()
    {
        StartCoroutine(SmoothStopPlay());
    }

    private IEnumerator ChangeMusic(AudioSource nextMusic)
    {
        while(_currentMusic.volume > _minVolume)
        {
            _currentMusic.volume = Mathf.MoveTowards(_currentMusic.volume, _minVolume, _volumeChangeSpeed * Time.deltaTime);
            yield return null;
        }

        _currentMusic.Stop();
        _currentMusic = nextMusic;
        _currentMusic.Play();

        while(_currentMusic.volume < _maxVolume)
        {
            _currentMusic.volume = Mathf.MoveTowards(_currentMusic.volume, _maxVolume, _volumeChangeSpeed * Time.deltaTime);
            yield return null;
        }

        yield break;
    }

    private IEnumerator SmoothStopPlay()
    {
        while (_currentMusic.volume > _minVolume)
        {
            _currentMusic.volume = Mathf.MoveTowards(_currentMusic.volume, _minVolume, _volumeChangeSpeed * Time.deltaTime);
            yield return null;
        }

        _currentMusic.Stop();
    }
}
