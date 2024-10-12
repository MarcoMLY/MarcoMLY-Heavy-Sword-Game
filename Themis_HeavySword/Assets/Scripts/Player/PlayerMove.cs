using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour, IStateable
{
    public bool Enabled { get; set; }

    [SerializeField] private float _moveSpeed;

    private CharacterManager _playerManager;
    private SetVelocity _setVelocity;

    private InputAction _move;

    private Vector2 _moveDirection;

    // Start is called before the first frame update
    void Awake()
    {
        _playerManager = GetComponent<CharacterManager>();
        _setVelocity = GetComponent<SetVelocity>();

        InputActionMap playerMap = _playerManager.InputAsset.FindActionMap("Player");
        playerMap.Enable();

        _move = playerMap.FindAction("Move");
    }

    private void OnEnable()
    {
        _move.performed += Move;
    }

    private void OnDisable()
    {
        _move.performed -= Move;
    }

    private void Update()
    {
        if (!Enabled)
            return;
        if (_move.WasReleasedThisFrame())
            _moveDirection = Vector2.zero;

        bool turnTo = _moveDirection != Vector2.zero;
        _setVelocity.ChangeVelocity(_moveDirection, turnTo);
    }

    // Update is called once per frame
    void Move(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>() * _moveSpeed;
    }

    public State HandleState()
    {
        return State.Continue;
    }

    public void OnStateEnabled()
    {
        _moveDirection = _move.ReadValue<Vector2>() * _moveSpeed;
    }
}
