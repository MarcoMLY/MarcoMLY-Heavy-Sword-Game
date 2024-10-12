using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;

public class NormalSpin : MonoBehaviour, IStateable
{
    [field:SerializeField] public bool Enabled { get; set; }

    private SetVelocity _setVelocity;
    [SerializeField] private float _normalDrag, _lowDragHighFPS, _lowDragLowFPS, _maxVelDecreaseDrag;
    private float _fpsAccountedLowDrag;
    [SerializeField] private Transform _tip;
    [SerializeField] private AnimationCurve _dragCurve;

    [SerializeField] private LayerMaskHolder _wall;

    private void Awake()
    {
        _setVelocity = GetComponent<SetVelocity>();
    }

    private void Update()
    {
        _fpsAccountedLowDrag = Mathf.Lerp(_lowDragLowFPS, _lowDragHighFPS, Time.captureFramerate / 60);
        if (!Enabled)
            return;
        _setVelocity.ChangeVelocity(_setVelocity.GetRBVelocity(), false);

        float t = _dragCurve.Evaluate(_setVelocity.GetRBVelocity().magnitude / _maxVelDecreaseDrag);

        float newDrag = Mathf.Lerp(_normalDrag, _fpsAccountedLowDrag, t);
        _setVelocity.ChangeDrag(newDrag);
    }

    public State HandleState()
    {
        return State.Continue;
    }

    public void OnStateEnabled() { }
}
