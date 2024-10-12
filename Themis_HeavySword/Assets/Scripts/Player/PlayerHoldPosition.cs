using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerHoldPosition : MonoBehaviour, IStateable
{
    public bool Enabled { get; set; }

    private CharacterManager _playerManager;
    private SetVelocity _setVelocity;

    private InputAction _holdPosition;

    [SerializeField] private UnityEvent _onPositionHeld;
    [SerializeField] private UnityEvent _onPositionReleased;
    [SerializeField] private GameEventBool _canThrowSword;

    [SerializeField] private VectorHolder _swordDirection;
    [SerializeField] private TransformHolder _sword;
    [SerializeField] private TransformHolder _camera;
    [SerializeField] private IntHolder _clockwise;

    [Range(0.9f, 1f)]
    [SerializeField] private float _dragMultiplier = 1f;
    [Range(0, 10)]
    [SerializeField] private float _turnAmount = 2f, angleTowardsClockwise;
    [Range(0, 1)]
    [SerializeField] private float _accuracy = 0.85f;

    private bool _slowingVelocity, _canThrow, _buttonReleased;
    private Camera _mainCam;

    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GetComponent<CharacterManager>();
        _setVelocity = GetComponent<SetVelocity>();

        InputActionMap playerMap = _playerManager.InputAsset.FindActionMap("Player");
        playerMap.Enable();

        _holdPosition = playerMap.FindAction("Sword");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_mainCam == null)
            _mainCam = _camera.Variable.gameObject.GetComponent<Camera>();
        if (!Enabled)
            return;
        if (_slowingVelocity)
            SlowVelocity(_setVelocity.GetRBVelocity());
    }

    public State HandleState()
    {
        Vector3 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = (mousePos - transform.position).normalized;
        if (_holdPosition.WasReleasedThisFrame() && SwordAndMouseDot(directionToMouse) > _accuracy && !_buttonReleased || _buttonReleased && _canThrow)
        {
            _onPositionReleased?.Invoke();
            _playerManager.PlayerSprite.parent = transform;
            _playerManager.PlayerSprite.position = transform.position;
            _canThrowSword.EventTriggered(false);
            return State.Change;
        }
        if (_holdPosition.WasReleasedThisFrame() && !_buttonReleased)
        {
            _buttonReleased = true;
            StopAllCoroutines();
            StartCoroutine(CheckThrow(directionToMouse));
        }
        return State.Continue;
    }

    private IEnumerator CheckThrow(Vector2 directionToMouse)
    {
        //float angleTowardsClockwise = 0.5f;
        Vector2 newDirectionToMouse = (directionToMouse.normalized + (directionToMouse.normalized.RotateVector2(-90 * _clockwise.Variable) * angleTowardsClockwise)).normalized;
        float radius = Vector2.Distance(transform.position, _sword.Variable.transform.position);
        Vector2 desirablePosition = (newDirectionToMouse.normalized * radius) + (Vector2)transform.position;
        Vector2 oldDirection = (desirablePosition - (Vector2)_sword.Variable.transform.position).normalized;
        float dotBetweenDirections = 1;
        //while (SwordAndMouseDot(directionToMouse) < _accuracy)
        while (dotBetweenDirections > 0.5f)
        {
            desirablePosition = (newDirectionToMouse.normalized * radius) + (Vector2)transform.position;
            Vector2 directionToDesirablePosition = (desirablePosition - (Vector2)_sword.Variable.transform.position).normalized;
            dotBetweenDirections = Vector2.Dot(directionToDesirablePosition, oldDirection);
            _canThrow = false;
            oldDirection = directionToDesirablePosition;
            yield return null;
        }
        _canThrow = true;
    }

    private float SwordAndMouseDot(Vector2 directionToMouse)
    {
        return Vector2.Dot(_swordDirection.Variable.normalized, directionToMouse);
    }

    public void OnStateEnabled()
    {
        _buttonReleased = false;
        _canThrow = false;
        _onPositionHeld?.Invoke();
        _playerManager.PlayerSprite.parent = _playerManager.Sword;
        SlowVelocity(_setVelocity.GetRBVelocity());
    }

    private void SlowVelocity(Vector2 currentVelocity)
    {
        _slowingVelocity = true;
        Vector2 newVelocity = currentVelocity;
        if (_slowingVelocity)
        {
            if (!Enabled)
                _slowingVelocity = false;
            float dragMultiplier = _dragMultiplier;
            newVelocity *= dragMultiplier;
            newVelocity = newVelocity.RotateVector2(_turnAmount * _playerManager.Clockwise * Time.deltaTime);
            _setVelocity.ChangeVelocity(newVelocity, false);
        }
    }
}
