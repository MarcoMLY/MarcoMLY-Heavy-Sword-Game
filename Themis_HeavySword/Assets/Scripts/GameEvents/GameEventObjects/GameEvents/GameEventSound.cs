using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameEvents/GameEventSound")]
public class GameEventSound : BasicGameEvent<ListenerSound>
{
    public void EventTriggered(AudioClip clip)
    {
        for (int i = 0; i < _listeners.Count; i++)
        {
            _listeners[i].TriggerEvent(clip);
        }
    }
}
