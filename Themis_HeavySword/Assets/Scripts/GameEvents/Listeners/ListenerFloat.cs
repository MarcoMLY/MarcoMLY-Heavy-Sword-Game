using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ListenerFloat : MonoBehaviour
{
    [SerializeField] private UnityEventFloat _event;
    [SerializeField] private GameEventFloat _gameEvent;

    private void OnEnable()
    {
        _gameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        _gameEvent.RemoveListener(this);
    }

    public void TriggerEvent(float info)
    {
        _event?.Invoke(info);
    }
}
