using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ListenerBool : MonoBehaviour
{
    [SerializeField] private UnityEventBool _event;
    [SerializeField] private GameEventBool _gameEvent;

    private void OnEnable()
    {
        _gameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        _gameEvent.RemoveListener(this);
    }

    public void TriggerEvent(bool info)
    {
        _event?.Invoke(info);
    }
}
