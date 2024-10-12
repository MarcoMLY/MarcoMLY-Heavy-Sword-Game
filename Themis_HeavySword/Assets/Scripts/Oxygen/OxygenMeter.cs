using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Data;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OxygenMeter : MonoBehaviour
{
    [SerializeField] private float _oxygenTime;
    private float _oxygenTimer;

    [SerializeField] private TransformHolder _player;

    [SerializeField] private LayerMask _safeZone;

    [SerializeField] private UnityEvent _onLooseOxygen;
    [SerializeField] private UnityEvent _onReturnHome;
    [SerializeField] private UnityEvent _onInsufficientMinerals;

    [SerializeField] private float _colliderRadius;

    [SerializeField] private TemporaryStorage[] _oxygenCrystals;
    [SerializeField] private DayDataHolder _dayDataHolder;

    [SerializeField] private FloatHolder _oxygenTimeLeft;
    [SerializeField] private FloatHolder _oxygenTimeHolder;

    private float _startOxygen;
    private bool _dayEnding = false;
    public bool PlayerInSafeZone { get; private set; }
    public bool InsufficientMinerals { get; private set; }

    private void Awake()
    {
        _oxygenTimeHolder.ChangeData(_oxygenTime);
        _oxygenTimer = _oxygenTime;
    }

    private void Start()
    {
        _startOxygen = _dayDataHolder.Day.Oxygen;

        if (_startOxygen + 5 < 5)
        {
            _oxygenTimer *= (_startOxygen + 5) / 5;
        }

        _oxygenTimeLeft.ChangeData(_oxygenTimer);
    }

    private void Update()
    {
        PlayerInSafeZone = PlayerEnterSafeZone();
        InsufficientMinerals = CheckfIfNeedOxygen();
        if (_dayEnding)
            return;
        _oxygenTimer -= Time.deltaTime;
        _oxygenTimeLeft.ChangeData(_oxygenTimer);
        if (_oxygenTimer > 0)
            return;
        if (CheckfIfInSafeZone())
        {
            _dayEnding = true;
            _onReturnHome?.Invoke();
        }
        _onLooseOxygen?.Invoke();
        _dayEnding = true;
    }

    private bool PlayerEnterSafeZone()
    {
        if (_dayEnding)
            return false;
        if (CheckfIfInSafeZone())
            return true;
        return false;
    }

    public void EndDay()
    {
        if (_dayEnding)
            return;
        if (!CheckfIfInSafeZone())
            return;
        if (CheckfIfNeedOxygen())
        {
            _onInsufficientMinerals?.Invoke();
            return;
        }

        _dayEnding = true;
        _onReturnHome?.Invoke();
    }

    private bool CheckfIfInSafeZone()
    {
        if (_player.Variable == null)
            return false;
        Collider2D[] hit = Physics2D.OverlapCircleAll(_player.Variable.position, _colliderRadius, _safeZone);
        return hit.Length >= 1;
    }

    private bool CheckfIfNeedOxygen()
    {
        if (_startOxygen > 0)
            return false;
        if (_dayDataHolder.CurrentDay > 0)
            return CheckIfNeedCrystals();

        return false;
    }

    private bool CheckIfNeedCrystals()
    {
        bool hasSavedCrystals = false;
        for (int i = 0; i < _oxygenCrystals.Length; i++)
        {
            if (_dayDataHolder.Day.MaterialsAndIndexes[_oxygenCrystals[i].MaterialType.Index] >= 1)
                hasSavedCrystals = true;
        }

        bool collectedCrystals = false;
        foreach (TemporaryStorage store in _oxygenCrystals)
        {
            if (store.GetAmount() >= 1)
            {
                collectedCrystals = true;
                break;
            }
        }

        if (!collectedCrystals && !hasSavedCrystals)
            return true;
        return false;
    }
}
