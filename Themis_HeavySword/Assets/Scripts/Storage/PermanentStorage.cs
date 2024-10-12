using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PermanentStorage
{
    public int Day;
    public int SaveSlot;

    public int[] CrystalHealths;
    public bool[] IsPermanentEnemyKilled;
    public int[] MaterialsAndIndexes;
    public float Oxygen;
    public float[] StartPosition;
    public int SwordUpgradeAmount;
    public bool[] HasSpecialAbilities;

    public PermanentStorage(int saveSlot, int day, int[] crystalHealths, bool[] isPermanentEnemyKilled, int[] materialAndIndexes, float oxygen, float[] startPosition, int swordUpgradeAmount, bool[] hasSpecialAbilities)
    {
        CrystalHealths = crystalHealths;
        IsPermanentEnemyKilled = isPermanentEnemyKilled;
        MaterialsAndIndexes = materialAndIndexes;
        Oxygen = oxygen;
        Day = day;
        SaveSlot = saveSlot;

        StartPosition = startPosition;
        SwordUpgradeAmount = swordUpgradeAmount;
        HasSpecialAbilities = hasSpecialAbilities;
    }
}
