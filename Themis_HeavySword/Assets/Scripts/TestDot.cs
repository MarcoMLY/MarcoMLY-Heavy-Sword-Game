using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDot : MonoBehaviour
{
    [SerializeField] private float _dotProduct;
    [SerializeField] private Vector3 _direction1;
    [SerializeField] private Vector3 _direction2;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + _direction1);
        Gizmos.DrawLine(transform.position, transform.position + _direction2);
        _dotProduct = Vector2.Dot(_direction1, _direction2);
    }
}
