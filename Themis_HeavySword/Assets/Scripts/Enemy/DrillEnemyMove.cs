using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillEnemyMove : EnemyMove
{
    [SerializeField] private float _pursuingTurnSpeed;
    private bool _hasShotDrill = false;

    protected override void Pursuing()
    {
        _enemyStateDebugger.ChangeColourState("Pursuing");

        _hasShotDrill = false;
        Vector3 direction = _player.Variable.position - transform.position;
        _enemySprite.up = Vector2.MoveTowards(_enemySprite.up, direction, Time.deltaTime * _pursuingTurnSpeed);

        float distance = Vector2.Distance(_currentTarget.position, transform.position);
        if (HasLineOfSight(false, _wallLayer.Variable) && distance <= _tooCloseDistance)
        {
            _moveAmount += _enemySprite.right.normalized;
            return;
        }
        _moveAmount += _enemySprite.up.normalized;
    }

    protected override void Attacking()
    {
        _enemyStateDebugger.ChangeColourState("Attacking");

        if (_hasShotDrill)
            return;
        DrillTurnInstant();
    }

    public void HasShotDrill()
    {
        _hasShotDrill = true;
    }

    private void DrillTurnInstant()
    {
        _enemySprite.up = (_currentTarget.position - transform.position).normalized;
    }
}
