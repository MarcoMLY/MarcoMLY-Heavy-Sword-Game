using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ListenerString : MonoBehaviour
{
    [SerializeField] private UnityEventString _event;
    [SerializeField] private GameEventString _gameEvent;

    private void OnEnable()
    {
        _gameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        _gameEvent.RemoveListener(this);
    }

    public void TriggerEvent(string info)
    {
        _event?.Invoke(info);
    }
}
