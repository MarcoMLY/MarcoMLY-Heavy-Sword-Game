using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimation : MonoBehaviour
{
    private CharacterManager _playerManager;
    Transform _playerSprite;

    [SerializeField] private float _turnSpeed;

    private Vector3 _direction;
    private bool _pauseTurn = false;

    private void Awake()
    {
        _playerManager = GetComponent<CharacterManager>();
        _playerSprite = _playerManager.PlayerSprite;
    }

    public void SetAnimation(Vector3 direction, bool turnToDirection)
    {
        if (!turnToDirection)
        {
            _pauseTurn = true;
            return;
        }
        _pauseTurn = false;

        if (direction == Vector3.zero)
            return;
        _direction = direction;
    }

    private void FixedUpdate()
    {
        if (!_pauseTurn)
            SmoothTurnTo(_direction);
    }

    private void SmoothTurnTo(Vector3 direction)
    {
        float newAngle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        float oldAngle = _playerSprite.eulerAngles.z;
        float towardsNewAngle = Mathf.LerpAngle(oldAngle, newAngle, Time.deltaTime * _turnSpeed);
        _playerSprite.rotation = Quaternion.Euler(0, 0, towardsNewAngle);
    }

    public void DirectTurnTo(Vector3 direction)
    {
        float newAngle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        _playerSprite.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    //public void SmoothTurnUpTo(Vector3 direction)
    //{
    //    Vector3 newUp = Vector3.SlerpUnclamped(_playerSprite.up, direction, Time.deltaTime * _turnSpeed);
    //    _playerSprite.up = newUp;
    //}
}
