using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Storage/Temporary")]
public class TemporaryStorage : ScriptableObject
{
    public MaterialType MaterialType { get => _materialType; }
    [SerializeField] private MaterialType _materialType;
    public int _amount;

    public int GetAmount()
    {
        return _amount;
    }

    public void ResetStorage()
    {
        _amount = 0;
    }

    public void StoreMaterial()
    {
        _amount += 1;
    }

    public void UseMaterials(int amount)
    {
        _amount -= amount;
        if (_amount < 0)
            _amount = 0;
    }
}
