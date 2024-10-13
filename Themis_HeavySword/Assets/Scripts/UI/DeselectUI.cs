using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeselectUI : DontDestroyOnLoadScene<DeselectUI>
{
    private EventSystem _eventSystem;

    private void OnLevelWasLoaded(int level)
    {
        _eventSystem = FindAnyObjectByType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_eventSystem == null)
            return;
        _eventSystem.SetSelectedGameObject(null);
    }
}
