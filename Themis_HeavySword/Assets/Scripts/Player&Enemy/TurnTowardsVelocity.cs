using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsVelocity : MonoBehaviour
{
    [SerializeField] private EnemyMove _mover;

    private void OnEnable()
    {
        _mover.OnMove += Turn;
    }

    private void OnDisable()
    {
        _mover.OnMove -= Turn;
    }

    private void Turn(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return;
        transform.up = direction;
    }
}
