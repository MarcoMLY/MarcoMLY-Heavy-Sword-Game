using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    [SerializeField] private Transform _spawnTransform, _parent;
    [SerializeField] private GameObject _gameObject;

    public void SpawnObject()
    {
        if (_parent == null)
        {
            Instantiate(_gameObject, _spawnTransform.position, _spawnTransform.rotation);
            return;
        }
        Instantiate(_gameObject, _spawnTransform.position, _spawnTransform.rotation, _parent);
    }
}
