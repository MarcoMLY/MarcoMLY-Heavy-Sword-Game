using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DontDestroyOnLoadScene<T> : MonoBehaviour  where T : MonoBehaviour
{
    private bool _origional;

    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        CheckOnLoad();
    }

    public virtual void CheckOnLoad()
    {
        DontDestroyOnLoad(gameObject);
        if (FindObjectsByType<T>(FindObjectsSortMode.None).Length > 1 && !_origional)
        {
            Destroy(gameObject);
        }
        _origional = true;
    }
}
