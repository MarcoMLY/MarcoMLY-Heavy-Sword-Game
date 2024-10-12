using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;

public class PlayerHealth : Health
{
    [SerializeField] private GameObject _hurtText;
    [SerializeField] private Vector3 _hurtTextOffset;
    [SerializeField] private float _criticalStrikeAmount;

    [SerializeField] private FloatHolder _currentHealth;
    [SerializeField] private FloatHolder _maxHealth;

    private void Start()
    {
        _maxHealth.ChangeData(TotalHealth);
        _currentHealth.ChangeData(TotalHealth);
    }

    private void Update()
    {
        _currentHealth.ChangeData(CurrentHealth);
    }

    public override bool Damage(float damage, GameObject attacker, bool overrideImmuneTime)
    {
        if (_immuneLayers.Contains(attacker.layer))
            return false;
        if (_immune && !overrideImmuneTime)
            return false;
        if (_temporaryInvinsibility == attacker.layer)
            return false;

        DamageText hurtText = Instantiate(_hurtText, transform.position + _hurtTextOffset, Quaternion.identity).GetComponent<DamageText>();
        hurtText.SpawnDamageText(damage, damage > _criticalStrikeAmount);

        _onDamaged?.Invoke();
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            _currentHealth.ChangeData(CurrentHealth);
            _onDie?.Invoke();
            return true;
        }

        _immune = true;

        StartCoroutine(DeImmune());
        return true;
    }
}
