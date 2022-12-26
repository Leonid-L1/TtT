using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Slider))]

public class Bar : MonoBehaviour
{
    [SerializeField] private bool _startValueIsMaxValue;

    private float _changeStep = 0.4f;
    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void SetMaxValue(int newMaxValue)
    {
        _slider.maxValue = (float)newMaxValue;

        if (_startValueIsMaxValue)
        {
            _slider.value = _slider.maxValue;
        }
        else
        {
            _slider.value = _slider.minValue;
        }
    }

    public void ChangeValue(int targetValue)
    {
        StartCoroutine(DoChangeBarValue(targetValue));
    }

    private IEnumerator DoChangeBarValue(int targetValue)
    {
        while (_slider.value != targetValue)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, targetValue, _changeStep);
            yield return null;
        }
        yield break;
    }
}
