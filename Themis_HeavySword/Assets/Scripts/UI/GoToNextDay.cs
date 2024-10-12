using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoToNextDay : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    [SerializeField] private GameEventReturnBool _sufficientOxygen;
    [SerializeField] private Animator _insufficuentOxygen;
    private bool _dayEnding = false;

    private void Update()
    {
        if (_dayEnding)
            return;
    }

    public void NextDay()
    {
        if (!_sufficientOxygen.RecieveMessages())
        {
            _insufficuentOxygen.SetTrigger("Prompt");
            return;
        }
        _dayEnding = true;
        _event?.Invoke();
    }
}
