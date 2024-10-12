using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Data;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class StuckInWall : MonoBehaviour, IStateable
{
    public bool Enabled { get; set; }

    [SerializeField] private TransformHolder _player;
    [SerializeField] private float _maxDistanceFromPlayer = 40f;

    [SerializeField] private Transform _collisionPoint;
    [SerializeField] private float _collisionRadius = 0.1f, _stickInEnemyAmount;
    [SerializeField] private Vector2 _enemyStickBox;
    [SerializeField] private LayerMask _wallLayer, _enemyStickLayer;

    private CharacterManager _manager;
    private SetVelocity _setVelocity;
    private SwordThrow _swordThrow;
    private SwordDamage _swordDamage;

    private InputAction _returnThrow;

    public Action<Collider2D> OnSwordStuck;
    public Action<Collider2D> OnSwordUnstuck;
    private Collider2D _currentStuckObject;

    [SerializeField] private UnityEvent _onHitWall;
    [SerializeField] private UnityEvent _onReturnToPlayer;
    [SerializeField] private UnityEvent _onStuckInEnemy;

    [SerializeField] private BoolHolder _returning;

    private bool _detecting = false, _stuck = false, _killedEnemy = false;

    private void Awake()
    {
        _manager = GetComponent<CharacterManager>();
        _setVelocity = GetComponent<SetVelocity>();
        _swordThrow = GetComponent<SwordThrow>();
        _swordDamage = GetComponent<SwordDamage>();

        InputActionMap playerMap = _manager.InputAsset.FindActionMap("Player");
        playerMap.Enable();

        _returnThrow = playerMap.FindAction("Sword");
    }

    private void OnEnable()
    {
        _swordThrow.ReturningToPlayer += StopDetection;
    }

    private void OnDisable()
    {
        _swordThrow.ReturningToPlayer -= StopDetection;
    }


    // Update is called once per frame
    void Update()
    {
        if (!Enabled)
            return;

        CheckForReturn();

        if (!_detecting)
            return;

        if (_player.Variable == null)
            return;

        if (!_stuck)
            CheckForOther();
        CheckForWall();
    }

    private void CheckForReturn()
    {
        if (_player.Variable == null)
            return;
        Collider2D[] box = Physics2D.OverlapCircleAll(_collisionPoint.position, _collisionRadius, _wallLayer);

        bool returnToPlayer = _returnThrow.WasPerformedThisFrame() || Vector2.Distance(transform.position, _player.Variable.position) > _maxDistanceFromPlayer;

        if (box.Length < 1 && !_stuck)
            return;
        if (!returnToPlayer)
            return;
        _onReturnToPlayer?.Invoke();
        if (_currentStuckObject != null)
            OnSwordUnstuck?.Invoke(_currentStuckObject);
        Unstuck();
    }

    private void CheckForWall()
    {
        Collider2D[] box = Physics2D.OverlapCircleAll(_collisionPoint.position, _collisionRadius, _wallLayer);

        if (box.Length >= 1)
        {
            _onHitWall?.Invoke();
            Stuck(null);
            return;
        }
    }

    public void CheckIfInEnemy(bool stuck)
    {
        if (!stuck)
            return;
        if (transform.parent != null)
            return;
        if (_returning.Variable)
            return;
        CheckForOther();
    }

    private void CheckForOther()
    {
        Collider2D[] SolidEnemies = Physics2D.OverlapBoxAll(_collisionPoint.position, _enemyStickBox, transform.eulerAngles.z, _enemyStickLayer);

        bool stuckInEnemy = false;
        foreach (Collider2D enemy in SolidEnemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>();

            if (enemyHealth == null)
            {
                transform.parent = enemy.transform;
                StickInEnemy(enemy);
                return;
            }
            if (enemyHealth.CurrentHealth <= _swordDamage.CheckDamageWillDeal())
            {
                OnSwordStuck?.Invoke(enemy);
                _killedEnemy = true;
                continue;
            }
            OnSwordStuck?.Invoke(enemy);
            ISwordStickable swordObsticle = enemy.GetComponent<ISwordStickable>();
            if (swordObsticle == null)
                continue;
            _killedEnemy = false;
            stuckInEnemy = true;
            transform.parent = swordObsticle.GetTransform();
            StickInEnemy(enemy);
            break;
        }

        if (stuckInEnemy)
        {
            _onStuckInEnemy?.Invoke();
        }
    }

    private void StickInEnemy(Collider2D stuckObject)
    {
        _onHitWall?.Invoke();
        Stuck(stuckObject);
        transform.position += (stuckObject.transform.position - transform.position).normalized * _stickInEnemyAmount;
    }

    private void Stuck(Collider2D stuckObject)
    {
        _currentStuckObject = stuckObject;
        _stuck = true;
        _setVelocity.ChangeVelocity(Vector2.zero, false);
        _setVelocity.DERotationalVelocity(false);
    }

    public void FreeSword()
    {
        Unstuck();
    }

    private void Unstuck()
    {
        _currentStuckObject = null;
        _stuck = false;
        transform.parent = null;
        _setVelocity.DERotationalVelocity(true);
    }

    public State HandleState()
    {
        if (_killedEnemy)
        {
            _killedEnemy = false;
            return State.Change;
        }
        return State.Continue;
    }

    private void StopDetection()
    {
        _detecting = false;
    }

    public void OnStateEnabled()
    {
        _detecting = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_collisionPoint.position, _collisionRadius);
        Gizmos.DrawWireCube(transform.position, _enemyStickBox);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
