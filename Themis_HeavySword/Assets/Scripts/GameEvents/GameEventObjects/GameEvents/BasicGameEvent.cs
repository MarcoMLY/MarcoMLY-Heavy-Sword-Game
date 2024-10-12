using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicGameEvent<T> : ScriptableObject
{
    protected List<T> _listeners = new List<T>();

    public virtual void AddListener(T listener)
    {
        _listeners.Add(listener);
    }

    public virtual void RemoveListener(T listener)
    {
        if (!_listeners.Contains(listener))
            return;
        _listeners.Remove(listener);
    }
}
