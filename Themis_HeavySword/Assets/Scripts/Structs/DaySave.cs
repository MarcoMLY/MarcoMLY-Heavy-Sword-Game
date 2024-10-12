using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DaySave
{
    public int[] CrystalHealths;
    public bool[] IsPermanentEnemyKilled;
    public int[] MaterialsAndIndexes;
    public float Oxygen;

    public float[] StartPosition;
    public int SwordUpgradeAmount;
    public bool[] HasSpecialAbilities;

    public DaySave(int[] crystalHealths, bool[] isPermanentEnemyKilled, int[] materialAndIndexes, float oxygen, float[] startPosition, int swordUpgradeAmount, bool[] hasSpecialAbilities)
    {
        CrystalHealths = crystalHealths;
        IsPermanentEnemyKilled = isPermanentEnemyKilled;
        MaterialsAndIndexes = materialAndIndexes;
        Oxygen = oxygen;
        StartPosition = startPosition;
        SwordUpgradeAmount = swordUpgradeAmount;
        HasSpecialAbilities = hasSpecialAbilities;
    }
}
