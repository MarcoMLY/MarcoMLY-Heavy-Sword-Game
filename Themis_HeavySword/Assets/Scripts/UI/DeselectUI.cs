using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeselectUI : DontDestroyOnLoadScene<DeselectUI>
{
    // Update is called once per frame
    void Update()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
