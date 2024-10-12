using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ListenerSound : MonoBehaviour
{
    [SerializeField] private UnityEventSound _event;
    [SerializeField] private GameEventSound _gameEvent;

    private void OnEnable()
    {
        _gameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        _gameEvent.RemoveListener(this);
    }

    public void TriggerEvent(AudioClip clip)
    {
        _event?.Invoke(clip);
    }
}
