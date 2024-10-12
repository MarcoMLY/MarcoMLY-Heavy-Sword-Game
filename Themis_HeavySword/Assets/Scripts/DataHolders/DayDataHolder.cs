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
}
