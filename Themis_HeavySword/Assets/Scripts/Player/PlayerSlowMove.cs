using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;
using UnityEngine.InputSystem;

public class PlayerSlowMove : MonoBehaviour, IStateable
{
    // Start is called before the first frame update
    public bool Enabled { get; set; }

    [SerializeField] private float _moveSpeed, _dragDivider;
    [SerializeField] private FloatConstantHolder _minSwordSpeed;

    [SerializeField] private FloatHolder _swordSpeed;
    [SerializeField] private GameEventBool _canThrowSword;

    private CharacterManager _playerManager;
    private SetVelocity _setVelocity;

    private InputAction _move, _holdPosition;

    private Vector2 _moveDirection;

    private bool _canSpin = false;

    // Start is called before the first frame update
    void Awake()
    {
        _playerManager = GetComponent<CharacterManager>();
        _setVelocity = GetComponent<SetVelocity>();

        InputActionMap playerMap = _playerManager.InputAsset.FindActionMap("Player");
        playerMap.Enable();

        _move = playerMap.FindAction("Move");

        _holdPosition = playerMap.FindAction("Sword");
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

        _canSpin = CheckIfCanSpin();

        if (_move.WasReleasedThisFrame())
            _moveDirection = Vector2.zero;

        bool turnTo = _moveDirection != Vector2.zero;
        Vector2 newVelocity = Vector2.zero;
        if (_moveDirection != Vector2.zero)
        {
            float lerpAmount = 1 - Mathf.Pow(0.5f, Time.deltaTime * _dragDivider);
            newVelocity = Vector3.Lerp(_setVelocity.Velocity, _moveDirection, lerpAmount);
        }
        _setVelocity.ChangeVelocity(newVelocity, turnTo);
    }

    private bool CheckIfCanSpin()
    {
        if (_swordSpeed.Variable >= _minSwordSpeed.Variable && !_canSpin)
            _canThrowSword.EventTriggered(true);
        if (_swordSpeed.Variable < _minSwordSpeed.Variable && _canSpin)
            _canThrowSword.EventTriggered(false);
        return _swordSpeed.Variable >= _minSwordSpeed.Variable;
    }

    // Update is called once per frame
    void Move(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>() * _moveSpeed;
    }

    public State HandleState()
    {
        if (_holdPosition.WasPerformedThisFrame() && _canSpin)
            return State.Change;
        return State.Continue;
    }

    public void OnStateEnabled()
    {
        _moveDirection = _move.ReadValue<Vector2>() * _moveSpeed;
        _canSpin = CheckIfCanSpin();
    }
}
