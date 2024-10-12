using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHolder : MonoBehaviour, IStateable
{
    public bool Enabled { get; set; }
    [SerializeReference] private MonoBehaviour _state1;
    [SerializeReference] private MonoBehaviour _state2;
    private IStateable _heldState1;
    private IStateable _heldState2;

    private void Awake()
    {
        _heldState1 = (IStateable)_state1;
        _heldState2 = (IStateable)_state2;
    }

    public State HandleState()
    {
        return State.Continue;
    }

    public void OnStateEnabled()
    {
        EnableStates();
    }

    private void Update()
    {
        if (!Enabled && (_heldState1.Enabled || _heldState2.Enabled))
        {
            DisableStates();
        }
    }

    private void EnableStates()
    {
        _heldState1.Enabled = true;
        _heldState2.Enabled = true;
        _heldState1.OnStateEnabled();
        _heldState2.OnStateEnabled();
    }

    private void DisableStates()
    {
        _heldState1.Enabled = false;
        _heldState2.Enabled = false;
    }
}
