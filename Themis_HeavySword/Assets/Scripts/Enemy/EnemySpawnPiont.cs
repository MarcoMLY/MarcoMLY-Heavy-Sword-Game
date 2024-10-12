using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPiont : MonoBehaviour
{
    [SerializeField] private float _spawnChance;
    [SerializeField] private GameObject _enemy;

    // Start is called before the first frame update
    void Awake()
    {
        if (Random.Range(0, 100) > _spawnChance)
            return;
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Instantiate(_enemy, transform.position, Quaternion.identity);
    }
}
