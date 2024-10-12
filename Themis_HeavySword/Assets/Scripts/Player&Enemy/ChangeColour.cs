using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _specialColour;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeToWhiteOrSpecialColour(bool specialColour)
    {
        _spriteRenderer.color = specialColour ? _specialColour : Color.white;
    }
}
