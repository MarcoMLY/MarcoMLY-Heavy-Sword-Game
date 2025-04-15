using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/MaterialUseButton")]
public class MaterialUseButton : ScriptableObject
{
    public int Id;
    public Sprite _buttonSprite;
    public float Amount;
    public MaterialType[] AdditionalMaterialsUsed;
    public float[] AdditionalMaterialAmounts;
}
