using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;
using UnityEngine.UI;

public class PlayerHealthRegenerationBar : MonoBehaviour
{
    private Slider _slider;
    [SerializeField] private float _lerpTime;
    [SerializeField] private FloatHolder _maxValue;

    // Start is called before the first frame update
    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void Regenerate(float amount)
    {
        float value = _slider.value + (amount / _maxValue.Variable);
        if (value > 1)
            value = 1;
        StartCoroutine(LerpTo(value));
    }

    private IEnumerator LerpTo(float value)
    {
        float timer = 0;
        while (timer <= _lerpTime)
        {
            timer += Time.deltaTime;
            _slider.value = Mathf.LerpUnclamped(_slider.value, value, timer / _lerpTime);
            yield return null;
        }

        _slider.value = value;
    }
}
