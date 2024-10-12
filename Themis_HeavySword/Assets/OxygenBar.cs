using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Data;
using UnityEngine.InputSystem;

public class OxygenBar : MonoBehaviour
{
    [SerializeField] private FloatHolder _maxValue;
    [SerializeField] private FloatHolder _currentValue;

    [SerializeField] private Gradient _colorGradient;
    [SerializeField] private Image[] _sliderFills;
    private float _yMin;
    private float _middle;

    private CanvasGroup _canvasGroup;
    [SerializeField] private AnimationCurve _fadeAnimation;
    [SerializeField] private float _fadeTime, _visibleTime;
    private float _visibleTimer;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void EnableOxygenBar()
    {
        if (_visibleTimer > 0)
        {
            StopAllCoroutines();
            _visibleTimer = 0;
            DisableOxygenBar();
            return;
        }
        StartCoroutine(Fade(true));
        _visibleTimer = _visibleTime;
    }

    public void DisableOxygenBar()
    {
        StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool fadeIn)
    {
        int startingPoint = fadeIn ? 0 : 1;

        float time = fadeIn ? 0 : _fadeTime;
        while (time >= 0 && time <= _fadeTime)
        {
            time += Time.deltaTime / _fadeTime * (fadeIn ? 1 : -1);
            _canvasGroup.alpha = _fadeAnimation.Evaluate(time);
            yield return null;
        }

        _canvasGroup.alpha = fadeIn ? 1 : 0;
    }

    private void Update()
    {
        if (_visibleTimer > 0)
        {
            _visibleTimer -= Time.deltaTime;
            if (_visibleTimer <= 0)
                DisableOxygenBar();
        }
        float value = _currentValue.Variable / _maxValue.Variable;
        ChangeColor(value);
        Image currentSlider = GetSliderToFill(value);
        if (currentSlider == null)
            return;
        FillSlider(currentSlider, value);
        DisableImages(_sliderFills, value);
    }

    private Image GetSliderToFill(float value)
    {
        int intValue = Mathf.FloorToInt(value * _sliderFills.Length);
        if (intValue == _sliderFills.Length || intValue < 0)
            return null;
        return _sliderFills[intValue];
    }

    private void FillSlider(Image slider, float value)
    {
        float totalValue = Mathf.FloorToInt(value * _sliderFills.Length);
        float sliderValue = (value * _sliderFills.Length) - totalValue;
        slider.transform.localScale = new Vector2(1, sliderValue);
    }

    private void ChangeColor(float value)
    {
        Color color = _colorGradient.Evaluate(value);
        for (int i = 0; i < _sliderFills.Length; i++)
        {
            _sliderFills[i].color = color;
        }
    }

    private void DisableImages(Image[] images, float value)
    {
        int intValue = Mathf.FloorToInt(value * _sliderFills.Length);
        if (intValue == _sliderFills.Length)
            return;
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(i <= (intValue));
        }
    }
}
