using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RunnerPhazeController : MonoBehaviour
{
    [SerializeField] private float _runnerPhazeDuration;

    public UnityAction RunnerPhazeEnded;
    private float _elapsedTime;

    void Update()
    {
        _elapsedTime +=Time.deltaTime;

        if(_elapsedTime >= _runnerPhazeDuration)
        {
            RunnerPhazeEnded?.Invoke();
        }
    }
}
