using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Data;

public class OxygenMineralController : MonoBehaviour
{
    [SerializeField] private IntHolder _saveSlot;
    [SerializeField] private float _maxOxygen;
    private float _oxygen;
    [SerializeField] private MaterialStorageHolder _materials;
    [SerializeField] private OxygenTanks _oxygenTanks;

    [SerializeField] private DayDataHolder _dayData;
    [SerializeField] private IntHolder _swordUpgradeAmount;

    [SerializeField] private GameEventReturnBool _isSufficientOxygen;

    [SerializeField] private UnityEvent _onOxygenFullyReplenished;

    // Start is called before the first frame update
    void Awake()
    {
        _oxygen = _dayData.Day.Oxygen;
        if (_oxygen < 0)
            _oxygen = 0;
        int[] materialAmounts = _dayData.Day.MaterialsAndIndexes;

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
            _onOxygenFullyReplenished?.Invoke();
        }
    }

    public void SaveData()
    {
        int[] materialAmounts = _dayData.Day.MaterialsAndIndexes;
        for (int i = 0; i < _materials.TemporaryStorages.Length; i++)
        {
            materialAmounts[i] = _materials.TemporaryStorages[i].GetAmount();
        }
        DaySave daySave = new DaySave(_dayData.Day.CrystalHealths, _dayData.Day.IsPermanentEnemyKilled, materialAmounts, _oxygen - 5, _dayData.Day.StartPosition, _swordUpgradeAmount.Variable, _dayData.Day.HasSpecialAbilities);
        _swordUpgradeAmount.ChangeData(0);
        SaveSystem.SaveData(_saveSlot.Variable, _dayData.CurrentDay + 1, daySave);
        _dayData.SetData(_dayData.CurrentDay + 1, daySave);

        _isSufficientOxygen.SendMessage -= EnoughOxygen;
    }

    private bool EnoughOxygen()
    {
        return _oxygen > 0;
    }
}
