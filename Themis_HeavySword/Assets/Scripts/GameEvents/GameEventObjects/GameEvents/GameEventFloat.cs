using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameEvents/GameEventFloat")]
public class GameEventFloat : BasicGameEvent<ListenerFloat>
{
    public void EventTriggered(float info)
    {
        for (int i = 0; i < _listeners.Count; i++)
        {
            _listeners[i].TriggerEvent(info);
        }
    }
}
