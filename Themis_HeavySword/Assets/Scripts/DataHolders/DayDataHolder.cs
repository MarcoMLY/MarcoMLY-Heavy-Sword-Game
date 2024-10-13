using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/DataHolders/DayDataHolder")]
public class DayDataHolder : ScriptableObject
{
    public DaySave Day { get; private set; }
    public int CurrentDay { get; private set; }

    public void SetData(int currentDay, DaySave day)
    {
        CurrentDay = currentDay;
        Day = day;
    }

    public void ChangeCurrentDay(int currentDay)
    {
        CurrentDay = currentDay;
    }

    public void ChangeStartPosition(float[] startPosition)
    {
        Day = new DaySave(Day.CrystalHealths, Day.IsPermanentEnemyKilled, Day.MaterialsAndIndexes, Day.Oxygen, startPosition, Day.SwordUpgradeAmount, Day.HasSpecialAbilities);
    }

    public void ChangeCrystalHealths(int[] crystalHealths)
    {
        Day = new DaySave(crystalHealths, Day.IsPermanentEnemyKilled, Day.MaterialsAndIndexes, Day.Oxygen, Day.StartPosition, Day.SwordUpgradeAmount, Day.HasSpecialAbilities);
    }

    public void ChangeIsPermenantEnemyKilled(bool[] isPermenantEnemyKilled)
    {
        Day = new DaySave(Day.CrystalHealths, isPermenantEnemyKilled, Day.MaterialsAndIndexes, Day.Oxygen, Day.StartPosition, Day.SwordUpgradeAmount, Day.HasSpecialAbilities);
    }

    public void ChangeSwordUpgradeAmount(int swordUpgradeAmount)
    {
        Day = new DaySave(Day.CrystalHealths, Day.IsPermanentEnemyKilled, Day.MaterialsAndIndexes, Day.Oxygen, Day.StartPosition, swordUpgradeAmount, Day.HasSpecialAbilities);
    }

    public void ChangeSwordSpecialAbilities(bool[] hasSpecialAbilities)
    {
        Day = new DaySave(Day.CrystalHealths, Day.IsPermanentEnemyKilled, Day.MaterialsAndIndexes, Day.Oxygen, Day.StartPosition, Day.SwordUpgradeAmount, hasSpecialAbilities);
    }
}
