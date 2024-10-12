using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;

public class SetDataTransform : MonoBehaviour
{
    [SerializeField] private TransformHolder _holder;
    [SerializeField] private Transform _data;

    private void Awake()
    {
        _holder.ChangeData(_data);
    }

    public void ChangeData(Transform newData)
    {
        _holder.ChangeData(newData);
    }
}
