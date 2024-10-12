using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public enum TimerType
//{
//    Normal,
//    Unscaled
//}

public class TimerManager : MonoBehaviour
{
    private List<Timer> _timers = new List<Timer>();
    //[SerializeField] private TimerType _type;
    private Func<float, float> _decreaseTimer;

    [SerializeField] private bool _debugTimers = false;

    private void Awake()
    {
        EndAllTimers();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = _timers.Count - 1; i >= 0; i--)
        {
            _timers[i].DecreaseTime(Time.deltaTime);
            if (_timers[i].Time > 0)
                continue;
            _timers[i].Function();
            _timers.RemoveAt(i);
        }

        if (_debugTimers)
        {
            Debug.Log(_timers.Count);
        }
    }

    //private float DecreaseTimerNormal(float time)
    //{
    //    time -= Time.deltaTime;
    //    return time;
    //}

    //private float DecreaseTimerUnscaled(float time)
    //{
    //    time -= Time.unscaledDeltaTime;
    //    return time;
    //}

    public void StartTimer(float time, Action function)
    {
        Timer timer = new Timer(time, function);
        _timers.Add(timer);
    }

    public bool FindTimer(Action function)
    {
        foreach (Timer timer in _timers)
        {
            if (function == timer.Function)
            {
                return true;
            }
        }
        return false;
    }

    public void EndAllTimers()
    {
        for (int i = _timers.Count - 1; i >= 0; i--)
        {
            _timers.RemoveAt(i);
        }
        _timers.Clear();
    }

    public void FinishAllTimers()
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            _timers[i].Function();
            _timers.Remove(_timers[i]);
        }
    }
}
