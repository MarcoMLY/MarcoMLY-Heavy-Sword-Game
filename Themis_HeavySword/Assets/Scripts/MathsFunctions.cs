using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathsFunctions
{
    public static Vector2 RotateVector2(this Vector2 direction, float angle)
    {
        angle *= -1;
        float x = (direction.x * Mathf.Cos(angle)) - (direction.y * Mathf.Sin(angle));
        float y = (direction.x * Mathf.Sin(angle)) + (direction.y * Mathf.Cos(angle));
        return new Vector2(x, y);
    }

    public static Vector3 RotateVector3(this Vector3 direction, float angle)
    {
        angle *= -1;
        float x = (direction.x * Mathf.Cos(angle)) - (direction.y * Mathf.Sin(angle));
        float y = (direction.x * Mathf.Sin(angle)) + (direction.y * Mathf.Cos(angle));
        return new Vector3(x, y, 0);
    }

    public static float GetGradient(this AnimationCurve curve, float time)
    {
        float point1 = curve.Evaluate(time);
        float point2 = curve.Evaluate(time + 0.01f);
        float angle = Vector2.Angle(new Vector2(point1, point1), new Vector2(point2, point2));
        float gradient = angle / 90;
        return gradient;
    }

    public static Vector3 DirectionFromAngle(this float angle, Vector3 axis)
    {
        //Quaternion myRotation = Quaternion.AngleAxis(angle, axis);

        //return myRotation * axis;
        return new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
    }
}
