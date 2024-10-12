using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Data;
using UnityEngine.Events;

public class SwordDamage : DamageSomething
{
    [SerializeField] private FloatConstantHolder _minSwordSpeed;
    [SerializeField] private BoolHolder _isPlayerHoldingSword, _isSwordRenturning;
    [SerializeField] private FloatHolder _swordSpeed;

    [SerializeField] private UnityEventBool _onDamageEnemy;

    [SerializeField] private float _criticalStrikeChance, _criticalStrikeDamage;

    private StuckInWall _stuckInWall;
    public Action<bool, float> OnCriticalStrike;
    private bool _stuck, _returningToPlayer, _isNextCriticalStrike;

    private void Awake()
    {
        _stuckInWall = GetComponent<StuckInWall>();
        _isPlayerHoldingSword.ChangeData(true);
        _isNextCriticalStrike = UnityEngine.Random.Range(0, 100) > _criticalStrikeChance;
    }

    private void OnEnable()
    {
        _stuckInWall.OnSwordStuck += DamageObject;
        _stuckInWall.OnSwordUnstuck += DamageObject;
    }

    private void OnDisable()
    {
        _stuckInWall.OnSwordStuck -= DamageObject;
        _stuckInWall.OnSwordUnstuck -= DamageObject;
    }

    // Update is called once per frame
    protected override void CheckCollider()
    {
        if (_swordSpeed.Variable < _minSwordSpeed.Variable && _isPlayerHoldingSword.Variable)
            return;

        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position + (transform.up * _offset.y) + (transform.right * _offset.x), _hitBox, transform.eulerAngles.z, _layer);

        if (_isPlayerHoldingSword.Variable || _isSwordRenturning.Variable)
        {
            DamageObjectOnImpactOnly(hit);
        }
    }

    private void DamageObjectOnImpactOnly(Collider2D[] objectsHit)
    {
        for (int i = 0; i < objectsHit.Length; i++)
        {
            IDameagable damageable = objectsHit[i].GetComponent<IDameagable>();

            if (damageable != null)
            {
                _onDamageEnemy?.Invoke(false);
                DamageWithCKChance(damageable, false);
            }
        }
    }

    private void DamageObject(Collider2D collider)
    {
        IDameagable damageable = collider.GetComponent<IDameagable>();

        if (damageable != null)
        {
            _onDamageEnemy?.Invoke(false);
            DamageWithCKChance(damageable, true);
        }
    }

    private bool IsInList(IDameagable single, IDameagable[] list)
    {
        foreach (IDameagable individual in list)
        {
            if (individual == single)
                return true;
        }
        return false;
    }

    private void DamageWithCKChance(IDameagable objectToDamage, bool overrideImmunitTime)
    {
        float damage = _isNextCriticalStrike ? _criticalStrikeDamage : _damage;
        if (objectToDamage.Damage(damage, gameObject, overrideImmunitTime))
            OnCriticalStrike?.Invoke(_isNextCriticalStrike, damage);
        _isNextCriticalStrike = UnityEngine.Random.Range(0, 101) <= _criticalStrikeChance;
    }

    public float CheckDamageWillDeal()
    {
        return _isNextCriticalStrike ? _criticalStrikeDamage : _damage;
    }
}
