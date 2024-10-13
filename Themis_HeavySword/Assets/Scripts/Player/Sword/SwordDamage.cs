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

    [SerializeField] private DayDataHolder _dayDataHolder;
    [SerializeField] private FloatHolder _miningDamageRandomChange;
    [SerializeField] private GameObject _extraDamageText;

    [SerializeField] private LayerMask _crystalLayer;
    private Collider2D _damagedEnemy;

    private StuckInWall _stuckInWall;
    public Action<bool, float> OnCriticalStrike;
    private bool _stuck, _returningToPlayer, _isNextCriticalStrike;
    private float _nextExtraDamage;

    private void Awake()
    {
        _stuckInWall = GetComponent<StuckInWall>();
        _isPlayerHoldingSword.ChangeData(true);
    }

    private void Start()
    {
        _isNextCriticalStrike = UnityEngine.Random.Range(0, 100) > _criticalStrikeChance;
        float randomChangeAmount = _miningDamageRandomChange.Variable;
        float extraDamage = (_isNextCriticalStrike ? _criticalStrikeDamage : _damage) * Mathf.CeilToInt(_dayDataHolder.Day.SwordUpgradeAmount * UnityEngine.Random.Range(1 - randomChangeAmount, 1 + randomChangeAmount));
        _nextExtraDamage = extraDamage;
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

    private void Update()
    {
        if (_damagedEnemy != null && !_isSwordRenturning.Variable)
            _damagedEnemy = null;
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
            if (objectsHit[i] == _damagedEnemy)
                return;
            IDameagable damageable = objectsHit[i].GetComponent<IDameagable>();

            if (damageable != null)
            {
                _onDamageEnemy?.Invoke(false);
                DamageWithCKChance(damageable, false, objectsHit[i].gameObject.layer);
            }

            if (_isSwordRenturning.Variable)
                _damagedEnemy = objectsHit[i];
        }
    }

    private float AddExtraMiningDamage()
    {
        float damage = 0;
        if (_dayDataHolder.Day.SwordUpgradeAmount > 0)
        {
            damage = _nextExtraDamage;
            SpawnExtraDamageText(_nextExtraDamage);

            float randomChangeAmount = _miningDamageRandomChange.Variable;
            float extraDamage = (_isNextCriticalStrike ? _criticalStrikeDamage : _damage) * Mathf.CeilToInt(_dayDataHolder.Day.SwordUpgradeAmount * UnityEngine.Random.Range(1 - randomChangeAmount, 1 + randomChangeAmount));
            _nextExtraDamage = extraDamage;
        }
        return damage;
    }

    private void SpawnExtraDamageText(float damageAmount)
    {
        DamageText damageText = Instantiate(_extraDamageText, transform.position, Quaternion.identity).GetComponent<DamageText>();
        damageText.SpawnDamageText(damageAmount, damageAmount >= _dayDataHolder.Day.SwordUpgradeAmount * 2);
    }

    private void DamageObject(Collider2D collider)
    {
        IDameagable damageable = collider.GetComponent<IDameagable>();

        if (damageable != null)
        {
            _damagedEnemy = collider;
            _onDamageEnemy?.Invoke(false);
            DamageWithCKChance(damageable, true, collider.gameObject.layer);
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

    private void DamageWithCKChance(IDameagable objectToDamage, bool overrideImmunitTime, int layer)
    {
        float damage = _isNextCriticalStrike ? _criticalStrikeDamage : _damage;
        if (_crystalLayer.Contains(layer))
            damage += AddExtraMiningDamage();
        if (objectToDamage.Damage(damage, gameObject, overrideImmunitTime))
            OnCriticalStrike?.Invoke(_isNextCriticalStrike, damage);
        _isNextCriticalStrike = UnityEngine.Random.Range(0, 101) <= _criticalStrikeChance;
    }

    public float CheckDamageWillDeal(int layer)
    {
        float damage = _isNextCriticalStrike ? _criticalStrikeDamage : _damage;
        if (_crystalLayer.Contains(layer))
            damage += _nextExtraDamage;
        return damage;
    }
}
