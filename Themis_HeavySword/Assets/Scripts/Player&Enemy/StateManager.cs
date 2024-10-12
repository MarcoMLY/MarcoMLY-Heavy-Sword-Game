using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour, IStateable
{
    public bool Enabled { get; set; } = true;
    [SerializeReference] private MonoBehaviour _state1;
    [SerializeReference] private MonoBehaviour _state2;
    private IStateable _currentState;
    private IStateable _disabledState;
    private bool _hasBeenDisabled;

    private void Awake()
    {
        _currentState = (IStateable)_state1;
        _disabledState = (IStateable)_state2;
        EnableStates(false);
    }

    private void OnEnable()
    {
        if (_hasBeenDisabled)
            EnableStates(false);
    }

    private void OnDisable()
    {
        DisableBothStates();
        _hasBeenDisabled = true;
    }

    private void Update()
    {
        if (!Enabled)
        {
            DisableBothStates();
            return;
        }
        State stateStays = _currentState.HandleState();
        if (stateStays == State.Change)
            SwitchStates();
    }

    public void SwitchStates()
    {
        IStateable current = _currentState;
        _currentState = _disabledState;
        _disabledState = current;
        EnableStates(true);
    }

    private void EnableStates(bool triggerEnableEvent)
    {
        _currentState.Enabled = true;
        _disabledState.Enabled = false;
        if (triggerEnableEvent)
            _currentState.OnStateEnabled();
    }

    private void DisableBothStates()
    {
        _currentState.Enabled = false;
        _disabledState.Enabled = false;
    }

    public void OnStateEnabled()
    {
        _currentState = (IStateable)_state1;
        _disabledState = (IStateable)_state2;
        EnableStates(true);
    }

    public State HandleState()
    {
        return State.Continue;
    }
}
