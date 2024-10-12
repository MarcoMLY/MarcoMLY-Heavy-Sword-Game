using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyMove : EnemyMove, ISwordStickable
{
    [SerializeField] protected float _attackWaitTime;

    protected override void SwitchStates()
    {
        if (_currentAction == Idle)
            _currentAction = CheckOnIdle();
        if (_currentAction == Pursuing)
            _currentAction = CheckOnPursuing();
        if (_currentAction == Attacking)
            _currentAction = CheckOnAttacking();
    }

    protected override Action CheckOnPursuing()
    {
        float _distance = Vector2.Distance(transform.position, _currentTarget.position);

        if (_distance > _stopPursuingDistance)
        {
            return Idle;
        }

        if (HasLineOfSight(true, _wallLayer.Variable))
        {
            StartCoroutine(AttackTimer());
            _onAttack?.Invoke();
            return Attacking;
        }

        return Pursuing;
    }

    protected override Action CheckOnAttacking()
    {
        float _distance = Vector2.Distance(transform.position, _currentTarget.position);

        if (_distance > _stopPursuingDistance)
        {
            StopAllCoroutines();
            return Idle;
        }

        if (!HasLineOfSight(true, _wallLayer.Variable))
        {
            StopAllCoroutines();
            return Pursuing;
        }

        return Attacking;
    }

    protected override void Pursuing()
    {
        _enemyStateDebugger.ChangeColourState("Pursuing");

        Vector3 direction = _agent.FindPathToPlayer();
        _enemySprite.up = Vector2.MoveTowards(_enemySprite.up, direction, Time.deltaTime * _turnSpeed);

        _moveAmount = direction;
    }

    protected override void Attacking()
    {
        _enemyStateDebugger.ChangeColourState("Attacking");

        Vector3 direction = _agent.FindPathToPlayer();

        float velDivider = 3f;
        Vector3 lookDirection = (_currentTarget.position + (_playerVelocity.Variable / velDivider)) - transform.position;
        _enemySprite.up = Vector2.MoveTowards(_enemySprite.up, lookDirection, Time.deltaTime * _turnSpeed);

        float distance = Vector2.Distance(_currentTarget.position, transform.position);
        if (distance <= _tooCloseDistance)
            return;
        _moveAmount += direction;
    }

    protected IEnumerator AttackTimer()
    {
        float timer = _attackWaitTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        OnAttack?.Invoke();
        StartCoroutine(AttackTimer());
    }

    public Transform GetTransform()
    {
        return _enemySprite;
    }
}
