using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;
using UnityEngine.Events;

public class SelectDay : MonoBehaviour
{
    [SerializeField] private SwitchBetweenDays _switchDays;
    [SerializeField] private IntHolder _saveSlot;

    [SerializeField] private UnityEvent _startSession;
    [SerializeField] private UnityEvent _startNewSession;

    public void PickDay()
    {
        SaveSystem.SaveDay(_saveSlot.Variable, _switchDays.Day);
    }

    public void OnClick()
    {
        if (_switchDays.Day <= 0)
        {
            _startNewSession?.Invoke();
            return;
        }
        _startSession?.Invoke();
    }
}
