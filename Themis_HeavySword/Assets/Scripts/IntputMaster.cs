using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class IntputMaster : DontDestroyOnLoadScene<IntputMaster>
{
    private InputAction _move;
    [SerializeField] private InputActionAsset _inputAsset;

    [SerializeField] private string[] _actions;
    private InputAction[] _inputActions;
    [SerializeField] private UnityEvent[] _onPerformed;
    [SerializeField] private UnityEvent[] _onReleased;

    [SerializeField] private string[] _vector1Actions;
    private InputAction[] _vector1InputActions;
    [SerializeField] private UnityEventFloat[] _onVector1Performed;

    protected override void OnAwake()
    {
        base.OnAwake();
        InputActionMap playerMap = _inputAsset.FindActionMap("Player");
        playerMap.Enable();

        _inputActions = new InputAction[_actions.Length];
        for (int i = 0; i < _actions.Length; i++)
        {
            _inputActions[i] = playerMap.FindAction(_actions[i]);
        }
        _vector1InputActions = new InputAction[_vector1Actions.Length];
        for (int i = 0; i < _vector1Actions.Length; i++)
        {
            _vector1InputActions[i] = playerMap.FindAction(_vector1Actions[i]);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _actions.Length; i++)
        {
            if (_inputActions[i].WasPerformedThisFrame())
                if (_onPerformed.Length > i)
                    _onPerformed[i]?.Invoke();
            if (_inputActions[i].WasReleasedThisFrame())
                if (_onReleased.Length > i)
                    _onReleased[i]?.Invoke();
        }

        for (int i = 0; i < _vector1Actions.Length; i++)
        {
            if (_onVector1Performed.Length <= i)
                return;
            float value = _vector1InputActions[i].ReadValue<float>();
            _onVector1Performed[i]?.Invoke(value);
        }
    }
}
