using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;

public class SetVelocityHolder : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private VectorHolder _velHolder;

    private void FixedUpdate()
    {
        _velHolder.ChangeData(_rb.velocity);
    }
}
