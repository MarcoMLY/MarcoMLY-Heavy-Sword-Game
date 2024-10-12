using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameEvents/GameEventString")]
public class GameEventString : BasicGameEvent<ListenerString>
{
    public void EventTriggered(string info)
    {
        for (int i = 0; i < _listeners.Count; i++)
        {
            _listeners[i].TriggerEvent(info);
        }
    }
}
