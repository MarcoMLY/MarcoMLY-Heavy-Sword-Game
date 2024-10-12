using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Data;
using UnityEngine.Events;
using UnityEngine.UI;

public class Oxygenprompts : MonoBehaviour
{
    [SerializeField] private FloatHolder _oxygenLeft;
    [SerializeField] private float _minutePromptTime;
    private bool _promptedOnMinute = false;

    [SerializeField] private TextMeshProUGUI _tenSecondsLeft;
    [SerializeField] private Animator _minuteLeft;
    [SerializeField] private Animator _insufficientMinerals;

    // Start is called before the first frame update
    void Awake()
    {
        _tenSecondsLeft.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_oxygenLeft.Variable < 0)
        {
            _tenSecondsLeft.enabled = false;
            return;
        }

        CheckMinute();
        CheckTenSeconds();
    }

    private void CheckMinute()
    {
        float min = 60;
        if (_promptedOnMinute)
            return;
        if (_oxygenLeft.Variable <= min)
        {
            _minuteLeft.SetTrigger("Prompt");
            _promptedOnMinute = true;
        }
    }

    private void CheckTenSeconds()
    {
        float tenSeconds = 11.5f;
        if (_oxygenLeft.Variable <= tenSeconds)
        {
            _tenSecondsLeft.enabled = true;
            _tenSecondsLeft.text = Mathf.Clamp(Mathf.FloorToInt(_oxygenLeft.Variable - 0.5f), 0, 10).ToString();
            return;
        }
        _tenSecondsLeft.enabled = false;
    }

    public void InsufficientMinerals()
    {
        _insufficientMinerals.SetTrigger("Prompt");
    }
}
