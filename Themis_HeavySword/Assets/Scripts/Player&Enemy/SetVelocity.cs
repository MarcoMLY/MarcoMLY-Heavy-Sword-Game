using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVelocity : MonoBehaviour
{
    private CharacterManager _playerManager;

    private PlayerAnimation _playerAnim;

    public Vector3 Velocity { get; private set; }

    private void Awake()
    {
        _playerManager = GetComponent<CharacterManager>();
        _playerAnim = GetComponent<PlayerAnimation>();
    }

    public void ChangeVelocity(Vector3 newVelocity, bool turn)
    {
        Velocity = newVelocity;
        if (_playerAnim != null)
            _playerAnim.SetAnimation(newVelocity, turn);
    }

    public void ChangeAngularVelocity(float newAngularVelocity)
    {
        _playerManager.Rb.angularVelocity = newAngularVelocity;
    }

    public void ResetVelocity(bool turn)
    {
        Velocity = Vector2.zero;
        _playerAnim.SetAnimation(Velocity, turn);
    }

    public void DEVelocity(bool disableOrEnable)
    {
        _playerManager.Rb.isKinematic = !disableOrEnable;
    }

    public void DERotationalVelocity(bool disableOrEnable)
    {
        _playerManager.Rb.freezeRotation = !disableOrEnable;
    }

    public void ChangeDrag(float drag)
    {
        _playerManager.Rb.drag = drag;
    }

    private void FixedUpdate()
    {
        _playerManager.Rb.velocity = Velocity;
    }

    public Vector2 GetRBVelocity()
    {
        return _playerManager.Rb.velocity;
    }

    public float GetRBAngularVelocity()
    {
        return _playerManager.Rb.angularVelocity;
    }
}
