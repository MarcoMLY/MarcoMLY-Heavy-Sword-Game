using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Data;
using UnityEngine.Events;

public class NextDayText : MonoBehaviour
{
    private TextMeshPro _text;
    //[SerializeField] private DayDataHolder _dayData;
    [SerializeField] private IntHolder _saveSlot;
    [SerializeField] private float _waitTime;

    [SerializeField] private UnityEvent _onTextFinished;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshPro>();
        //_text.text = "Day " + _dayData.CurrentDay;
        int day = SaveSystem.LoadDayNumber(_saveSlot.Variable);
        if (day == -1)
            day = 0;
        _text.text = "Day " + (day);

        StartCoroutine(TextAnimate());
    }

    private IEnumerator TextAnimate()
    {
        yield return new WaitForSeconds(_waitTime);
        _onTextFinished?.Invoke();
    }
}
