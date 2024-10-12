using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;

public class SwordData : MonoBehaviour
{
    [SerializeField] private IntHolder _clockwise;
    [SerializeField] private FloatHolder _swordSpeed;

    private SetVelocity _setVelocity;

    private Vector2 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        _setVelocity = GetComponent<SetVelocity>();
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _clockwise.ChangeData(CalculateClockwise());
        _swordSpeed.ChangeData(CalculateSpeed());
    }

    private int CalculateClockwise()
    {
        Vector2 movingDirection = _setVelocity.Velocity.normalized;
        Vector2 rightDirection = transform.right.normalized;
        float dot = Vector2.Dot(movingDirection, rightDirection);
        if (dot > 0)
            return 1;
        return -1;
    }

    private float CalculateSpeed()
    {
        return _setVelocity.GetRBVelocity().magnitude;
    }
}
