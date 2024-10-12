using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _spawnWait, _timeBetweenAttacks;
    [SerializeField] private int _numberOfAttacks;
    [SerializeField] private UnityEvent _onShoot;
    [SerializeField] private Transform _spawnProjectile, _enemySprite;

    [SerializeField] private bool _dontShootOtherEnemies;
    [SerializeField] private LayerMaskHolder _enemy;
    [SerializeField] private TransformHolder _player;
    private EnemyMove _move;

    private void Awake()
    {
        _move = GetComponent<EnemyMove>();
    }

    private void OnEnable()
    {
        _move.OnAttack += OnAttack;
    }

    private void OnDisable()
    {
        _move.OnAttack -= OnAttack;
    }

    public void OnAttack()
    {
        StartCoroutine(Shoot());
    }

    protected bool HasLineOfSightDistanceVectorEnemy(Vector3 shootDirection)
    {
        if (_player.Variable == null)
            return false;
        float distance = (transform.position - _player.Variable.transform.position).magnitude;
        RaycastHit2D[] ray;
        ray = Physics2D.RaycastAll(transform.position, shootDirection.normalized, distance, _enemy.Variable);
        return ray.Length <= 1;
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(_spawnWait);

        for (int i = 0; i < _numberOfAttacks; i++)
        {
            if (!_dontShootOtherEnemies || HasLineOfSightDistanceVectorEnemy(_enemySprite.up))
                _onShoot?.Invoke();
            yield return new WaitForSeconds(_timeBetweenAttacks);
        }
    }
}
