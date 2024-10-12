using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Data;

public class LerpToSlider : MonoBehaviour
{
    [SerializeField] private float _lerpMultiplier;
    private Slider _slider;

    [SerializeField] private Slider _otherSlider;

    // Start is called before the first frame update
    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        UpdateSlider(_otherSlider.value);
    }

    // Update is called once per frame
    void UpdateSlider(float value)
    {
        _slider.value = Mathf.LerpUnclamped(_slider.value, value, Time.deltaTime * _lerpMultiplier);
    }
}
