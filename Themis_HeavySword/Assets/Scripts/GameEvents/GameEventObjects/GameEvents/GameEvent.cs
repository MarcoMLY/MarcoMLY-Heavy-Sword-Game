using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameEvents/GameEvent")]
public class GameEvent : BasicGameEvent<Listener>
{
    public void EventTriggered()
    {
        for (int i = 0; i < _listeners.Count; i++)
        {
            _listeners[i].TriggerEvent();
        }
    }
}
