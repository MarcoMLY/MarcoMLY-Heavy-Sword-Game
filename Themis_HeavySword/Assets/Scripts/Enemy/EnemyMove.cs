using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.Data;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class EnemyMove : MonoBehaviour
{
    protected Transform _currentTarget;
    protected PathFind _agent;

    protected Action _currentAction;

    [Space(3)]
    [Header("Enemy Information")]
    [SerializeField] protected TransformHolder _player;
    [SerializeField] protected VectorHolder _playerVelocity;
    [SerializeField] protected Transform _enemySprite;
    [SerializeField] protected StatesDebugger _enemyStateDebugger;
    [SerializeField] protected LayerMaskHolder _wallLayer, _enemyLayer;

    [Space(3)]
    [Header("Enemy interation distances")]
    [SerializeField] protected float _noticeDistance, _stopPursuingDistance, _tooCloseDistance, _idlePosMoveAmount = 2f, _idleMoveWaitTime = 5f, _idleSlowMultiplier = 0.2f;
    private float _idleMoveWaitTimer;

    [Space(3)]
    [Header("Enemy stats")]
    [SerializeField] protected float _turnSpeed;
    [SerializeField] protected float _pursiuSpeed = 5f;
    [SerializeField] protected float _attackTime = 4f;
    [SerializeField] protected float _pursuiTime = 4f;
    [SerializeField] protected float _enemyRadius = 0.5f;

    [Header("Enemy Events")]
    [SerializeField] protected UnityEvent _onIdle;
    [SerializeField] protected UnityEvent _onPursui;
    [SerializeField] protected UnityEvent _onAttack;
    [HideInInspector] public Action OnAttack;

    protected Vector3 _moveAmount, _idlePosition, _idleMoveToPosition;
    [HideInInspector] public Action<Vector2> OnMove;
    protected bool _timerOver = false;


    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<PathFind>();
        _idlePosition = transform.position;
    }

    private void OnDisable()
    {
        _currentAction = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentAction == null)
        {
            _onIdle?.Invoke();
            _currentAction = Idle;
        }
        Vector3 totalVelocity = Vector3.zero;

        if (_player.Variable == null)
        {
            StopAllCoroutines();
            Idle();
            Move(_moveAmount);
            return;
        }
        _currentTarget = _player.Variable;
        _currentAction.Invoke();
        Move(_moveAmount);

        SwitchStates();
    }

    protected virtual void SwitchStates()
    {
        if (_currentAction == Idle)
            _currentAction = CheckOnIdle();
        if (_currentAction == Pursuing)
            _currentAction = CheckOnPursuing();
        if (_currentAction == Attacking)
            _currentAction = CheckOnAttacking();

        if (_currentAction != Idle)
            _idleMoveToPosition = transform.position;
    }

    protected virtual Action CheckOnIdle()
    {
        float _distance = Vector2.Distance(transform.position, _currentTarget.position);

        if (_distance > _noticeDistance)
        {
            _idlePosition = transform.position;
            return Idle;
        }
        if (!HasLineOfSight(false, _wallLayer.Variable))
        {
            _idlePosition = transform.position;
            return Idle;
        }

        _onPursui?.Invoke();
        StartCoroutine(Timer(_pursuiTime));
        return Pursuing;
    }

    protected virtual Action CheckOnPursuing()
    {
        float _distance = Vector2.Distance(transform.position, _currentTarget.position);

        if (_distance > _stopPursuingDistance)
        {
            _onIdle?.Invoke();
            StopAllCoroutines();
            _idlePosition = transform.position;
            return Idle;
        }

        if (_timerOver && HasLineOfSight(false, _wallLayer.Variable))
        {
            OnAttack?.Invoke();
            _onAttack?.Invoke();
            StopAllCoroutines();
            StartCoroutine(Timer(_attackTime));
            return Attacking;
        }

        return Pursuing;
    }

    protected virtual Action CheckOnAttacking()
    {
        if (_timerOver)
        {
            StopAllCoroutines();
            StartCoroutine(Timer(_pursuiTime));
            _onPursui?.Invoke();
            return Pursuing;
        }

        return Attacking;
    }

    protected virtual void Pursuing()
    {
        _enemyStateDebugger.ChangeColourState("Pursuing");
        float moveAwaySpeed = _pursiuSpeed / 10f;

        float distance = Vector2.Distance(transform.position, _currentTarget.position);
        Vector3 playerDirection = (_currentTarget.position - transform.position).normalized;
        Vector3 direction = _agent.FindPathToPlayer();

        _enemySprite.up = Vector2.MoveTowards(_enemySprite.up, direction, Time.deltaTime * _turnSpeed);

        if (distance <= _tooCloseDistance && HasLineOfSight(true, _wallLayer.Variable))
        {
            if (_tooCloseDistance - distance < 0.1f)
                return;
            Vector3 moveDirection = -playerDirection.normalized * moveAwaySpeed;
            if (HasLineOfSightVector(transform.position + moveDirection, false, _wallLayer.Variable))
                _moveAmount += moveDirection;

            _enemySprite.up = Vector2.MoveTowards(_enemySprite.up, (Vector2)playerDirection, Time.deltaTime * _turnSpeed);
            moveDirection = playerDirection.RotateVector3(90).normalized * moveAwaySpeed;
            if (HasLineOfSightVector(transform.position + moveDirection, true, _wallLayer.Variable))
                _moveAmount += moveDirection;
            return;
        }   

        if (distance >= _tooCloseDistance || !HasLineOfSight(true, _wallLayer.Variable))
        {
            _moveAmount += direction;
        }
    }

    public virtual void ChasePlayer()
    {
        float distance = Vector2.Distance(transform.position, _currentTarget.position);
        if (_currentAction == Attacking)
            return;
        if (distance <= _stopPursuingDistance)
        {
            _onPursui?.Invoke();
            if (_pursuiTime != 0)
            {
                StopAllCoroutines();
                StartCoroutine(Timer(_pursuiTime));
            }
            _currentAction = Pursuing;
        }
    }

    protected virtual void Idle()
    {
        _enemyStateDebugger.ChangeColourState("Idle");
        _idleMoveWaitTimer -= Time.deltaTime;
        if (_idleMoveWaitTimer <= 0)
        {
            float waitTimeRandomizer = 0.2f;
            _idleMoveWaitTimer = _idleMoveWaitTime + UnityEngine.Random.Range(-waitTimeRandomizer, waitTimeRandomizer);
            _idleMoveToPosition = _idlePosition + new Vector3(UnityEngine.Random.Range(-_idlePosMoveAmount, _idlePosMoveAmount), UnityEngine.Random.Range(-_idlePosMoveAmount, _idlePosMoveAmount), 0);
        }
        if (Vector2.Distance(transform.position, _idleMoveToPosition) < 0.05f)
            return;
        if (!HasLineOfSightVector(_idleMoveToPosition, true, _wallLayer.Variable))
            return;
        if (_idleMoveWaitTimer > 0)
        {
            Vector3 moveDirection = (_idleMoveToPosition - transform.position).normalized * _idleSlowMultiplier;
            _enemySprite.up = Vector2.MoveTowards(_enemySprite.up, moveDirection, Time.deltaTime * _turnSpeed);
            _moveAmount += moveDirection;
        }
    }

    protected virtual void Attacking()
    {
        _enemyStateDebugger.ChangeColourState("Attacking");

        float velDivider = 3f;
        Vector3 lookDirection = (_currentTarget.position + (_playerVelocity.Variable / velDivider)) - transform.position;
        _enemySprite.up = Vector2.MoveTowards(_enemySprite.up, lookDirection, Time.deltaTime * _turnSpeed);
    }

    protected bool HasLineOfSight(bool circleCast, LayerMask layerMask)
    {
        Vector2 rayDireciton = _currentTarget.position - transform.position;
        RaycastHit2D[] ray;
        if (circleCast)
        {
            ray = Physics2D.CircleCastAll(transform.position, _enemyRadius, rayDireciton, rayDireciton.magnitude, layerMask);
            return ray.Length <= 0;
        }
        ray = Physics2D.RaycastAll(transform.position, rayDireciton.normalized, rayDireciton.magnitude, layerMask);
        return ray.Length <= 0;
    }

    protected bool HasLineOfSightVector(Vector3 point, bool circleCast, LayerMask layerMask)
    {
        Vector2 rayDireciton = point - transform.position;
        RaycastHit2D[] ray;
        if (circleCast)
        {
            ray = Physics2D.CircleCastAll(transform.position, _enemyRadius, rayDireciton, rayDireciton.magnitude, layerMask);
            return ray.Length <= 0;
        }
        ray = Physics2D.RaycastAll(transform.position, rayDireciton.normalized, rayDireciton.magnitude, layerMask);
        return ray.Length <= 0;
    }

    protected bool HasLineOfSightDistanceDirectionEnemy(float distance, Vector3 direction, bool circleCast)
    {
        RaycastHit2D[] ray;
        if (circleCast)
        {
            ray = Physics2D.CircleCastAll(transform.position, _enemyRadius, direction, distance, _enemyLayer.Variable);
            return ray.Length <= 1;
        }
        ray = Physics2D.RaycastAll(transform.position, direction, distance, _enemyLayer.Variable);
        return ray.Length <= 1;
    }

    protected virtual IEnumerator Timer(float seconds)
    {
        _timerOver = false;
        yield return new WaitForSeconds(seconds);
        _timerOver = true;
    }

    protected void Move(Vector3 direction)
    {
        if (HasLineOfSightDistanceDirectionEnemy(_enemyRadius * 4, direction.normalized, true))
        {
            transform.position += direction * _pursiuSpeed * Time.deltaTime;
            OnMove?.Invoke(_moveAmount);
        }
        _moveAmount = Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _noticeDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _stopPursuingDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _tooCloseDistance);
    }
}
