using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlPrompt : MonoBehaviour
{
    private TextMeshPro _text;
    private string _currentControlSender;

    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
    }

    public void RecieveControlPrompt(string control)
    {
        string[] splitString = control.Split('|');
        _text.text = splitString[0];
        if (splitString.Length <= 1)
            return;
        _currentControlSender = splitString[1];
    }

    public void EndControlPrompt(string controlEnd)
    {
        string[] splitString = controlEnd.Split('|');
        if (splitString.Length <= 1)
            return;
        if (_currentControlSender == splitString[1])
            _text.text = "";
    }
}
