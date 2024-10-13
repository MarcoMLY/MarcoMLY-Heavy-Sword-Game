using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MaterialAtBase : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    private SpriteRenderer _renderer;
    public int Amount { get; private set; }
    bool _oxygenTanksFull = false;

    [SerializeField] private MaterialType _materialType;
    [SerializeField] private TemporaryStorage _materialStorage;
    [SerializeField] private UnityEvent _onCrystalUsed;
    [SerializeField] private UnityEvent _onMaterialClicked;

    private void Awake()
    {
        SetMaterialAmount(_materialStorage.GetAmount());
    }

    public void SetMaterialAmount(int amount)
    {
        _renderer = GetComponent<SpriteRenderer>();
        Amount = amount;
        SetVisuals();
    }

    private void SetVisuals()
    {
        if (Amount <= 0)
            _renderer.enabled = false;
        if (Amount > 0)
            _renderer.enabled = true;
        _text.text = Amount.ToString();
        _text.enabled = _renderer.enabled;
    }

    public void OxygenFullyReplenished()
    {
        _oxygenTanksFull = true;
    }

    public void OnMouseDown()
    {
        if (Amount <= 0)
            return;
        _onMaterialClicked?.Invoke();
        if (!_oxygenTanksFull)
            _onCrystalUsed?.Invoke();
        SetVisuals();
    }

    public void DecreaseAmount()
    {
        Amount -= 1;
        if (Amount <= 0)
            Amount = 0;
        _materialStorage.UseMaterials(1);
    }
}
