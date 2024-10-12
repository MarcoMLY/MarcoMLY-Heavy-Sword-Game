using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Data;

public class ConrtolTemporaryStorage : MonoBehaviour
{
    [SerializeField] private MaterialStorageHolder _temporaryStorages;
    [SerializeField] private SetGameData _gameData;
    [SerializeField] private IntHolder _saveSlot;
    [SerializeField] private DayDataHolder _dayHolder;
    [SerializeField] private Vector3Holder[] _spawnPoints;

    private void Awake()
    {
        int day = GetCurrentDay(_saveSlot.Variable);
        if (day <= 0)
            day = 0;

        LoadData(_saveSlot.Variable, day);
    }

    public void UseMaterials(int[] amounts)
    {
        for (int i = 0; i < _temporaryStorages.TemporaryStorages.Length; i++)
        {
            _temporaryStorages.TemporaryStorages[i].UseMaterials(amounts[i]);
        }
    }

    public void SaveData()
    {
        SavePlayer(_saveSlot.Variable, _dayHolder.CurrentDay);
    }

    public void SavePlayer(int saveSlot, int day)
    {
        int[] temporaryStorage = new int[_temporaryStorages.TemporaryStorages.Length];
        for (int i = 0; i < temporaryStorage.Length; i++)
        {
            temporaryStorage[i] = _temporaryStorages.TemporaryStorages[i].GetAmount();
        }
        DaySave daySave = new DaySave(_gameData.GetCrystalData(), _gameData.GetPerminantEnemyData(), temporaryStorage, _dayHolder.Day.Oxygen, _spawnPoints[1].Variable, _dayHolder.Day.SwordUpgradeAmount, _dayHolder.Day.HasSpecialAbilities);
        _dayHolder.SetData(day, daySave);
    }

    public void LoadData(int saveSlot, int day)
    {
        PermanentStorage data = SaveSystem.LoadData(saveSlot, day);
        if (day > 0)
            _gameData.SetScene(data.CrystalHealths, data.IsPermanentEnemyKilled);
        _dayHolder.SetData(day, new DaySave(data.CrystalHealths, data.IsPermanentEnemyKilled, data.MaterialsAndIndexes, data.Oxygen, data.StartPosition, data.SwordUpgradeAmount, data.HasSpecialAbilities));
        for (int i = 0; i < _temporaryStorages.TemporaryStorages.Length; i++)
        {
            _temporaryStorages.TemporaryStorages[i]._amount = data.MaterialsAndIndexes[i];
        }
    }

    private int GetCurrentDay(int saveSlot)
    {
        int day = SaveSystem.LoadDayNumber(saveSlot);
        if (day == -1)
            return 0;
        return day;
    }
}
