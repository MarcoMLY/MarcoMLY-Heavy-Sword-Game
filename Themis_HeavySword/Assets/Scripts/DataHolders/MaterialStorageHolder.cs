using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Storage/TemporaryStorages")]
public class MaterialStorageHolder : ScriptableObject
{
    public TemporaryStorage[] TemporaryStorages;
}
