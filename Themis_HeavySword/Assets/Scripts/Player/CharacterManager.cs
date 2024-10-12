using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour
{
    public InputActionAsset InputAsset;

    [SerializeField] private IntHolder _clockwise;
    [SerializeField] private FloatHolder _swordSpeed;
    [SerializeField] private TransformHolder _sword;

    [SerializeField] private Rigidbody2D _rb;
    public Rigidbody2D Rb { get => _rb; }

    public int Clockwise { get; private set; }
    public float SwordSpeed { get; private set; }
    public Transform Sword { get; private set; }
    public Transform PlayerSprite;

    private void FixedUpdate()
    {
        Clockwise = _clockwise.Variable;
        SwordSpeed = _swordSpeed.Variable;
        Sword = _sword.Variable;
    }
}
