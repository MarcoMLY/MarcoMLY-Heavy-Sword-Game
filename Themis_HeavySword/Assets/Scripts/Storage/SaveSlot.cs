using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSlot
{
    public int CurrentDay;
    public SaveSlot(int day)
    {
        CurrentDay = day;
    }
}
