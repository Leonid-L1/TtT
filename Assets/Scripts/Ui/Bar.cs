using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]

public class Bar : MonoBehaviour
{
    [SerializeField] private bool _startValueIsMaxValue;
    [SerializeField] private float _changeStep = 0.4f;
    [SerializeField] private Slider _slider;

    public float Value => _slider.value;

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
