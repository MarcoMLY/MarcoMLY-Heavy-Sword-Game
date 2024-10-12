using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StatesDebugger : MonoBehaviour
{
    [SerializeField] private Color[] _colours;
    [SerializeField] private string[] _stateNames;
    private Color _currentColor;
    private bool _colourTemporaryChange = false;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeColourState(string stateName)
    {
        int index = -1;
        for (int i = 0; i < _stateNames.Length; i++)
        {
            if (stateName.ToLower() == _stateNames[i].ToLower())
            {
                index = i;
            }
        }

        if (index >= 0)
        {
            if (!_colourTemporaryChange)
                _spriteRenderer.color = _colours[index];
            _currentColor = _colours[index];
        }
    }

    public void ChangeColourto0or1(bool state)
    {
        if (_colours.Length < 1)
            return;
        if (!_colourTemporaryChange)
            _spriteRenderer.color = _colours[state ? 1 : 0];
        _currentColor = _colours[state ? 1 : 0];
    }

    public void SetToWhiteTemporarily(float time)
    {
        StartCoroutine(TemporaryColourChange(time, Color.white));
    }

    private IEnumerator TemporaryColourChange(float time, Color color)
    {
        _colourTemporaryChange = true;
        _spriteRenderer.color = color;
        yield return new WaitForSeconds(time);
        _spriteRenderer.color = _currentColor;
        _colourTemporaryChange = false;
    }
}
