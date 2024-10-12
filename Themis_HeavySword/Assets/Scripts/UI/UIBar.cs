using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    private Slider _slider;
    [SerializeField] private FloatHolder _maxValue;
    [SerializeField] private FloatHolder _currentValue;

    [SerializeField] private Gradient _colorGradient;
    [SerializeField] private Image _sliderImage;
    [SerializeField] private Image _sliderBackround;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        float value = _currentValue.Variable / _maxValue.Variable;
        _slider.value = value;
        _sliderImage.color = _colorGradient.Evaluate(value);
    }
}
