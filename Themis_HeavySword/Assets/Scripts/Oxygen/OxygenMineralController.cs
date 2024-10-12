using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Data;

public class OxygenMineralController : MonoBehaviour
{
    [SerializeField] private IntHolder _saveSlot;
    [SerializeField] private float _maxOxygen;
    private float _oxygen;
    [SerializeField] private MaterialAtBase[] _materials;
    [SerializeField] private OxygenTanks _oxygenTanks;

    [SerializeField] private DayDataHolder _dayData;

    [SerializeField] private GameEventReturnBool _isSufficientOxygen;

    // Start is called before the first frame update
    void Awake()
    {
        _oxygen = _dayData.Day.Oxygen;
        if (_oxygen < 0)
            _oxygen = 0;
        int[] materialAmounts = _dayData.Day.MaterialsAndIndexes;
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetMaterialAmount(materialAmounts[i]);
        }

        _oxygenTanks.SetOxygen(_oxygen);
    }

    private void OnEnable()
    {
        _isSufficientOxygen.SendMessage += EnoughOxygen;
    }

    private void OnDisable()
    {
        _isSufficientOxygen.SendMessage -= EnoughOxygen;
    }

    private void Update()
    {
        if (_isSufficientOxygen.EventNull())
            _isSufficientOxygen.SendMessage += EnoughOxygen;
    }

    public void CrystalUsed()
    {
        _oxygen += 1;
        if (_oxygen >= _maxOxygen)
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i].OxygenFullyReplenished();
            }
        }
    }

    public void SaveData()
    {
        int[] materialAmounts = _dayData.Day.MaterialsAndIndexes;
        for (int i = 0; i < _materials.Length; i++)
        {
            materialAmounts[i] = _materials[i].Amount;
        }
        DaySave daySave = new DaySave(_dayData.Day.CrystalHealths, _dayData.Day.IsPermanentEnemyKilled, materialAmounts, _oxygen - 5, _dayData.Day.StartPosition, _dayData.Day.SwordUpgradeAmount, _dayData.Day.HasSpecialAbilities);
        SaveSystem.SaveData(_saveSlot.Variable, _dayData.CurrentDay + 1, daySave);
        _dayData.SetData(_dayData.CurrentDay + 1, daySave);

        _isSufficientOxygen.SendMessage -= EnoughOxygen;
    }

    private bool EnoughOxygen()
    {
        return _oxygen > 0;
    }
}
