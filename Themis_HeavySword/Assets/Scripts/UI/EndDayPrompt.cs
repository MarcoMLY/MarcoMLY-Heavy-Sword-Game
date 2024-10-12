using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndDayPrompt : MonoBehaviour
{
    [SerializeField] private OxygenMeter _oxygenMeter;
    [SerializeField] private TextMeshProUGUI _endDayText;
    [SerializeField] private TextMeshProUGUI _insufficientMineralsTest;

    private void Awake()
    {
        _endDayText.enabled = false;
        _insufficientMineralsTest.enabled = false;
    }

    private void Update()
    {
        if (_oxygenMeter == null)
            return;
        if (!_oxygenMeter.PlayerInSafeZone)
        {
            _endDayText.enabled = false;
            _insufficientMineralsTest.enabled = false;
            return;
        }
        _endDayText.enabled = !_oxygenMeter.InsufficientMinerals;
        _insufficientMineralsTest.enabled = _oxygenMeter.InsufficientMinerals;
    }
}
