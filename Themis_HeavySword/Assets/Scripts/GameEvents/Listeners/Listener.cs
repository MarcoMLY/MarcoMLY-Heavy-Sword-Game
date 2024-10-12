using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Listener : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    [SerializeField] private GameEvent _gameEvent;

    private void OnEnable()
    {
        _gameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        _gameEvent.RemoveListener(this);
    }

    public void TriggerEvent()
    {
        _event?.Invoke();
    }
}
