using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameData : MonoBehaviour
{
    [SerializeField] private Health[] _crystals;
    [SerializeField] private Health[] _permanentEnemies;

    public void SetScene(int[] crystalHealths, bool[] isPermanentEnemyKilled)
    {
        for (int i = 0; i < _crystals.Length; i++)
        {
            if (crystalHealths[i] <= 0)
            {
                _crystals[i].SetStartingHealth(0);
                _crystals[i].gameObject.SetActive(false);
                continue;
            }
            _crystals[i].SetStartingHealth(crystalHealths[i]);
        }

        for (int i = 0; i < _permanentEnemies.Length; i++)
        {
            if (isPermanentEnemyKilled[i])
            {
                _permanentEnemies[i].SetStartingHealth(0);
                _permanentEnemies[i].gameObject.SetActive(false);
            }
        }
    }

    public int[] GetCrystalData()
    {
        int[] crystalHealths = new int[_crystals.Length];
        for (int i = 0; i < _crystals.Length; i++)
        {
            if (_crystals[i].CurrentHealth == 0)
            {
                crystalHealths[i] = 0;
                continue;
            }
            crystalHealths[i] = (int)_crystals[i].CurrentHealth;
        }
        return crystalHealths;
    }

    public bool[] GetPerminantEnemyData()
    {
        bool[] isPermanentEnemyKilled = new bool[_permanentEnemies.Length];
        for (int i = 0; i < _permanentEnemies.Length; i++)
        {
            isPermanentEnemyKilled[i] = false;
            if (_permanentEnemies[i].CurrentHealth == 0)
            {
                isPermanentEnemyKilled[i] = true;
                continue;
            }
        }
        return isPermanentEnemyKilled;
    }
}
