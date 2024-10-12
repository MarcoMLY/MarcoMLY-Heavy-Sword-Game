using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Data;
using TMPro;

public class SaveSlotButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private IntListHolder _saveSlotDays;
    [SerializeField] private int _saveSlotIndex;

    public UnityEvent _onClickNewSlot;
    public UnityEvent _onClickUsedSlot;

    private int _days;

    private void OnEnable()
    {
        _days = _saveSlotDays.Variable[_saveSlotIndex];
        switch (_days)
        {
            case <= 0:
                _text.text = "Start new game";
                break;
            case > 0:
                _text.text = "continue from day " + _days + "\n or pick day";
                break;
        }
    }

    public void CheckIfUsed()
    {
        if (_days <= 0)
        {
            _onClickNewSlot?.Invoke();
            return;
        }
        _onClickUsedSlot?.Invoke();
    }
}
