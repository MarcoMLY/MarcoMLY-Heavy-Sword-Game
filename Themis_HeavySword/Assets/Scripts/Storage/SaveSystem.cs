using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class SaveSystem
{
    public static void SaveData(int saveSlot, int day, DaySave daySave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string metaDataPath = Application.persistentDataPath + "/SaveSlot" + saveSlot + "Days.themisData";
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + "Day" + day + ".themisData";
        FileStream metaDatastream = new FileStream(metaDataPath, FileMode.Create);
        FileStream stream = new FileStream(path, FileMode.Create);

        PermanentStorage data = new PermanentStorage(saveSlot, day, daySave.CrystalHealths, daySave.IsPermanentEnemyKilled, daySave.MaterialsAndIndexes, daySave.Oxygen, daySave.StartPosition, daySave.SwordUpgradeAmount, daySave.HasSpecialAbilities);

        formatter.Serialize(metaDatastream, new SaveSlot(day));
        formatter.Serialize(stream, data);
        metaDatastream.Close();
        stream.Close();
    }

    public static void SaveDay(int saveSlot, int day)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string metaDataPath = Application.persistentDataPath + "/SaveSlot" + saveSlot + "Days.themisData";

        FileStream metaDatastream = new FileStream(metaDataPath, FileMode.Create);

        formatter.Serialize(metaDatastream, new SaveSlot(day));
        metaDatastream.Close();
    }

    public static int LoadDayNumber(int saveSlot)
    {
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + "Days.themisData";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveSlot day = formatter.Deserialize(stream) as SaveSlot;
            stream.Close();
            return day.CurrentDay;
        }

        Debug.Log("Error, save file not found in " + path);
        return -1;
    }

    public static PermanentStorage LoadData(int saveSlot, int day)
    {
        string path = Application.persistentDataPath + "/SaveSlot" + saveSlot + "Day" + day + ".themisData";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            PermanentStorage data = formatter.Deserialize(stream) as PermanentStorage;
            stream.Close();
            return data;
        }

        Debug.Log("Error, save file not found in " + path);
        return new PermanentStorage(0, -1, new int[0], new bool[0], new int[0], 0, new float[0], 0, new bool[0]);
    }
}
