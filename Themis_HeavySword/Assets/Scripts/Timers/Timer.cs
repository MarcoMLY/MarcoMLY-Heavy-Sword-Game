using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Timer
{
    private static float _time;
    public float Time
    {
        get => _time;
        set => _time = value;
    }
    public Action Function;

    public Timer(float time, Action function)
    {
        _time = time;
        Function = function;
    }

    public void DecreaseTime(float amount)
    {
        Time -= amount;
    }
}
