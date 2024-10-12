using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;

public class SelectDay : MonoBehaviour
{
    [SerializeField] private SwitchBetweenDays _switchDays;
    [SerializeField] private IntHolder _saveSlot;

    public void PickDay()
    {
        SaveSystem.SaveDay(_saveSlot.Variable, _switchDays.Day);
    }
}
