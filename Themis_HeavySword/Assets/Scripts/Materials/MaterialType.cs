using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Materials")]
public class MaterialType : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public int Index;
    [SerializeField] public Color Color;
    [SerializeField] public Sprite Sprite;
}
