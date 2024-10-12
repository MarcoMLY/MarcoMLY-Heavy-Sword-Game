using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;

public class SaveSlotController : MonoBehaviour
{
    [SerializeField] private IntListHolder _saveSlotDays;
    [SerializeField] private IntHolder _saveSlot;
    [SerializeField] private int _saveSlots;
    [SerializeField] private MaterialStorageHolder _materialsAndIndexes;
    [SerializeField] private float _oxygen;
    [SerializeField] private Vector3Holder _startPos;
    [SerializeField] private int _upgradeAmount;
    [SerializeField] private bool[] _hasSpecialAbilities;

    // Start is called before the first frame update
    void Awake()
    {
        _saveSlotDays.ClearData();
        List<int> saveSlotDays = new List<int>();
        for (int i = 0; i < _saveSlots; i++)
        {
            int day = SaveSystem.LoadDayNumber(i);
            if (day == -1)
                day = 0;
            _saveSlotDays.AddData(day);
        }
    }

    public void SaveData()
    {
        int[] materials = new int[_materialsAndIndexes.TemporaryStorages.Length];
        DaySave daySave = new DaySave(new int[0], new bool[0], materials, _oxygen, _startPos.Variable, _upgradeAmount, _hasSpecialAbilities);
        SaveSystem.SaveData(_saveSlot.Variable, 0, daySave);
    }
}
