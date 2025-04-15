using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseMaterial : MonoBehaviour
{
    [field: SerializeField] public MaterialUseButton _materialUse { get; private set; }
    public Action<MaterialUseButton> OnClicked;

    public void Use()
    {
        OnClicked?.Invoke(_materialUse);
    }
}
