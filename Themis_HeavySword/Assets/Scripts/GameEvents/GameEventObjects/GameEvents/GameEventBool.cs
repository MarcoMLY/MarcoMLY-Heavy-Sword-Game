using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameEvents/GameEventBool")]
public class GameEventBool : BasicGameEvent<ListenerBool>
{
    public void EventTriggered(bool info)
    {
        for (int i = 0; i < _listeners.Count; i++)
        {
            _listeners[i].TriggerEvent(info);
        }
    }
}
