using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;
using UnityEngine.Events;

public class AutoSpin : MonoBehaviour, IStateable
{
    public bool Enabled { get; set; }

    private float _spinMagnitude;

    private SetVelocity _setVelocity;
    private HingeJoint2D _joint;

    [SerializeField] private IntHolder _clockwise;
    [SerializeField] private Transform _handlePivot;
    [SerializeField] private float _spinSpeedMultiplier = 1.2f;

    [SerializeField] private UnityEvent _onAutoSpin;

    private void Awake()
    {
        _setVelocity = GetComponent<SetVelocity>();
        _joint = GetComponent<HingeJoint2D>();
    }

    private void Update()
    {
        if (!Enabled)
            return;
        _setVelocity.ChangeVelocity(CalculateVelocity(), false);
    }

    public State HandleState()
    {
        return State.Continue;
    }

    public void OnStateEnabled()
    {
        _onAutoSpin.Invoke();
        _joint.anchor = _handlePivot.localPosition;
        _spinMagnitude = _setVelocity.Velocity.magnitude * _spinSpeedMultiplier;
    }

    private Vector2 CalculateVelocity()
    {
        return transform.right * _clockwise.Variable * _spinMagnitude;
    }
}
