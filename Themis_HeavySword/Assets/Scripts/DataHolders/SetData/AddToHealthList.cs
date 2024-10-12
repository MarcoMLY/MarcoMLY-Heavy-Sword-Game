using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;

public class AddToHealthList : MonoBehaviour
{
    [SerializeField] private HealthListHolder _healthList;
    private Health _health;

    private void Awake()
    {
        _healthList.AddData(_health);
    }

    private void OnDisable()
    {
        _healthList.RemoveData(_health);
    }
}
