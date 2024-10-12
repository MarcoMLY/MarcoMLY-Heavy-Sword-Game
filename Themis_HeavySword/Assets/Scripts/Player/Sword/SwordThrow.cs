using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Data;
using Unity.VisualScripting;

public class SwordThrow : MonoBehaviour, IStateable
{
    public bool Enabled { get; set; }

    private SetVelocity _setVelocity;
    private CharacterManager _manager;

    [SerializeField] private TransformHolder _player, _camera;
    [SerializeField] private Transform _swordReturnTransform;
    private HingeJoint2D _joint;

    [SerializeField] private float _playerCatchRadius, _throwSpeedMultiplier, _returnSpeed, _apexTime, _aimAssist;

    [SerializeField] private AnimationCurve _ThrowCurve;

    private Keyframe _apex;
    private Vector2 _startDirection;
    private Vector2 _startPosition;

    private float _startSpeed;
    private float _throwTimer;

    private Action _currentAction;
    public Action ReturningToPlayer;

    [SerializeField] private UnityEvent _onReturnToPlayer, _onThrow;

    [Space(2)]
    [Header("Super throw")]
    [SerializeField] private float _superThrowSpeedMultiplier;
    [SerializeField] private FloatConstantHolder _superThowSwordSpeed;
    [SerializeField] private BoolHolder _returningToPlayer;

    private void Awake()
    {
        _setVelocity = GetComponent<SetVelocity>();
        _manager = GetComponent<CharacterManager>();
        _joint = GetComponent<HingeJoint2D>();
        _returningToPlayer.ChangeData(false);
        FindApexKey();         
    }

    private void FindApexKey()
    {
        Keyframe highestKey = _ThrowCurve.keys[0];
        for (int i = 0; i < _ThrowCurve.keys.Length; i++)
        {
            Keyframe key = _ThrowCurve.keys[i];
            if (key.value > highestKey.value)
            {
                highestKey = key;
            }
        }
        _apex = highestKey;
    }

    private void Update()
    {
        if (_player.Variable == null)
            return;

        if (!Enabled)
        {
            OnDisabled();
            return;
        }

        _currentAction?.Invoke();
    }

    private void OnDisabled()
    {
        if (transform.parent == null)
        {
            transform.parent = _player.Variable;
            _joint.enabled = true;
            ResetSword();
        }
    }

    public State HandleState()
    {
        return State.Continue;
    }

    public void OnStateEnabled()
    {
        transform.parent = null;
        _joint.enabled = false;

        Vector2 currentVelocity = _setVelocity.Velocity;

        _startDirection = currentVelocity.normalized;
        _startDirection = AddAimAssist(_startDirection).normalized;
        _startPosition = transform.position;

        _startSpeed = currentVelocity.magnitude;
        _throwTimer = 0f;

        _setVelocity.DEVelocity(false);
        _currentAction = Throwing;
        _onThrow?.Invoke();
    }

    private Vector2 AddAimAssist(Vector2 direction)
    {
        Camera main = _camera.Variable.gameObject.GetComponent<Camera>();
        Vector2 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = (mousePos - (Vector2)transform.position).normalized;

        Vector2 newDirection = Vector2.Lerp(direction, directionToMouse, _aimAssist).normalized;
        return newDirection;
    }

    private void Throwing()
    {
        float multiplier = _throwSpeedMultiplier;
        if (_startSpeed >= _superThowSwordSpeed.Variable)
            multiplier = _superThrowSpeedMultiplier;
        if (_throwTimer < _apex.time)
        {
            _throwTimer += Time.deltaTime;
            Vector2 displacement = _startDirection * _startSpeed * _ThrowCurve.Evaluate(_throwTimer) * _throwSpeedMultiplier;
            Vector2 position = _startPosition + displacement;

            transform.position = position;
            _setVelocity.ChangeVelocity(Vector2.zero, false);
            return;
        }

        StartCoroutine(ThrowApex());
    }
    
    private IEnumerator ThrowApex()
    {
        yield return new WaitForSeconds(_apexTime);
        ReturningToPlayer?.Invoke();

        _throwTimer = _apex.time;
        _currentAction = Returning;
    }

    private void Returning()
    {
        if (Vector2.Distance(transform.position, _player.Variable.position) > _playerCatchRadius)
        {
            _returningToPlayer.ChangeData(true);

            _throwTimer = Mathf.Clamp(_throwTimer - Time.deltaTime, 0, Mathf.Infinity);
            float speed = Mathf.Clamp((_apex.value - _ThrowCurve.Evaluate(_throwTimer)) * _returnSpeed, 0, _returnSpeed);

            Vector2 direction = (_player.Variable.position - transform.position).normalized;
            Vector2 displacement = direction * speed;

            TurnToPlayer(direction);

            transform.position += (Vector3)displacement * Time.deltaTime;
            _setVelocity.ChangeVelocity(Vector2.zero, false);
            return;
        }

        _returningToPlayer.ChangeData(false);
        _onReturnToPlayer?.Invoke();
        _setVelocity.DEVelocity(true);
        _setVelocity.DERotationalVelocity(true);
    }

    private void TurnToPlayer(Vector2 playerDirection)
    {
        float spinDragMultiplier = 8f;
        float pointToPlayerSpeed = 6;

        if (Mathf.Abs(_setVelocity.GetRBAngularVelocity()) >= 30f)
        {
            _setVelocity.ChangeAngularVelocity(Mathf.Lerp(_setVelocity.GetRBAngularVelocity(), 0, Time.deltaTime * spinDragMultiplier));
            return;
        }
        _setVelocity.DERotationalVelocity(false);
        transform.up = Vector3.Slerp(transform.up, -playerDirection, Time.deltaTime * pointToPlayerSpeed);
    }

    private void ResetSword()
    {
        Transform playerT = _player.Variable;
        float rotation = transform.eulerAngles.z;
        transform.up = playerT.transform.up;

        transform.position = _swordReturnTransform.position;

        _joint.anchor = Vector3.zero - _swordReturnTransform.localPosition;
        _joint.connectedAnchor = Vector3.zero;

        transform.RotateAround(playerT.position, Vector3.forward, rotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (transform.parent != null)
            Gizmos.DrawWireSphere(transform.parent.position, _playerCatchRadius);

        FindApexKey();
        float adveradgeSpeed = 27 * _throwSpeedMultiplier;
        float distanceTravelled = _apex.value * adveradgeSpeed;
        Vector3 throwVector = transform.up * distanceTravelled;
        if (transform.parent != null)
            Gizmos.DrawLine(transform.parent.position, transform.parent.position + throwVector);
    }

    public void PauseThrow()
    {
        _currentAction = null;
    }

    public void ContinueThrow()
    {
        _currentAction = Throwing;
    }

    public void ReturnThrow()
    {
        _throwTimer = _apex.time;
        _currentAction = Returning;
        ReturningToPlayer?.Invoke();
    }
}
