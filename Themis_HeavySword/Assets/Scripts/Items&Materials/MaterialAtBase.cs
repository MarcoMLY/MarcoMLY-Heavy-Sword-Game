using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MaterialAtBase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    public int Amount { get; private set; }
    bool _oxygenTanksFull = false;

    [field: SerializeField] public MaterialType MaterialType { get; private set; }
    [field: SerializeField] public TemporaryStorage MaterialStorage { get; private set; }

    [field: SerializeField] public MaterialUseButton MaterialUseButtons { get; private set; }

    private void OnEnable()
    {
        SetMaterialAmount(MaterialStorage.GetAmount());
    }

    public void SetMaterialAmount(int amount)
    {
        Amount = amount;
        SetVisuals();
    }

    private void SetVisuals()
    {
        _text.text = Amount.ToString();
    }

    public void OxygenFullyReplenished()
    {
        _oxygenTanksFull = true;
    }


    public void DecreaseAmount()
    {
        Amount -= 1;
        if (Amount <= 0)
            Amount = 0;
    }
}
