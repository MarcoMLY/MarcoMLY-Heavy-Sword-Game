using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ControlPrompt : MonoBehaviour
{
    private TextMeshPro _text;
    private List<string> _controlPrompts = new List<string>();
    private List<int> _importances = new List<int>();

    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        ShowControlPrompts();
    }

    private void ShowControlPrompts()
    {
        string mostImportantPrompt = "";
        int importance = 0;

        for (int i = 0; i < _importances.Count; i++)
        {
            if (_importances[i] >= importance)
            {
                mostImportantPrompt = _controlPrompts[i];
                importance = _importances[i];
            }
        }
        _text.text = mostImportantPrompt;
    }

    public void RecieveControlPrompt(string control)
    {
        string[] splitString = control.Split('|');
        _controlPrompts.Add(splitString[0]);
        _importances.Add(int.Parse(splitString[1]));
    }

    public void EndControlPrompt(string controlEnd)
    {
        string[] splitString = controlEnd.Split('|');
        _controlPrompts.Remove(splitString[0]);
        _importances.Remove(int.Parse(splitString[1]));
    }
}
