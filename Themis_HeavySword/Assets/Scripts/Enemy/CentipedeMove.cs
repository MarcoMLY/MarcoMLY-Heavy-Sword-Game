using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeMove : EnemyMove, ISwordStickable
{
    [SerializeField] private float _pursuingTurnSpeed;
    private bool _hasShotBeam = false;

    protected override void Pursuing()
    {
        _enemyStateDebugger.ChangeColourState("Pursuing");

        _hasShotBeam = false;
        Vector3 direction = _agent.FindPathToPlayer();
        _enemySprite.up = Vector2.MoveTowards(_enemySprite.up, direction, Time.deltaTime * _pursuingTurnSpeed);

        float distance = Vector2.Distance(_currentTarget.position, transform.position);
        if (HasLineOfSight(false, _wallLayer.Variable) && distance <= _tooCloseDistance)
        {
            return;
        }
        _moveAmount += _enemySprite.up.normalized;
    }

    protected override void Attacking()
    {
        _enemyStateDebugger.ChangeColourState("Attacking");

        if (_hasShotBeam)
            return;
        float velDivider = 3f;
        Vector3 lookDirection = (_currentTarget.position + (_playerVelocity.Variable / velDivider)) - transform.position;
        _enemySprite.up = Vector2.MoveTowards(_enemySprite.up, lookDirection, Time.deltaTime * _turnSpeed);
    }

    public void HasShotBeam()
    {
        _hasShotBeam = true;
    }

    public Transform GetTransform()
    {
        return _enemySprite;
    }
}
