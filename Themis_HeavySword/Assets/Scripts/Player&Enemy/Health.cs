using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Data;

public class Health : MonoBehaviour, IDameagable
{
    [field: SerializeField] public float TotalHealth { get; protected set; }
    [field: SerializeField] public float CurrentHealth { get; protected set; }

    [SerializeField] protected UnityEvent _onDamaged;
    [SerializeField] protected UnityEvent _onDie;

    [SerializeField] protected LayerMask _immuneLayers;
    [SerializeField] protected float _immuneTime;

    protected bool _immune = false;

    protected int _temporaryInvinsibility;

    private void Awake()
    {
        if (CurrentHealth == 0)
            CurrentHealth = TotalHealth;

        _temporaryInvinsibility = _immuneLayers;
    }

    public void SetStartingHealth(float startingHealth)
    {
        CurrentHealth = startingHealth;
    }

    public virtual bool Damage(float damage, GameObject attacker, bool overrideImmuneTime)
    {
        if (_immuneLayers.Contains(attacker.layer))
            return false;
        if (_immune && !overrideImmuneTime)
            return false;
        if (_temporaryInvinsibility == attacker.layer)
            return false;

        _onDamaged?.Invoke();
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            _onDie?.Invoke();
            return true;
        }

        _immune = true;

        StartCoroutine(DeImmune());
        return true;
    }

    protected IEnumerator DeImmune()
    {
        yield return new WaitForSeconds(_immuneTime);
        _immune = false;
    }

    public virtual void DestroyUnit()
    {
        Destroy(gameObject);
    }

    public void DestroyParent()
    {
        Destroy(transform.parent.gameObject);
    }

    public void ChangeTemporaryInvinsibility(int layer)
    {
        _temporaryInvinsibility = layer;
    }

    public void RemoveTemporaryInvinsibility()
    {
        _temporaryInvinsibility = -1;
    }
}
