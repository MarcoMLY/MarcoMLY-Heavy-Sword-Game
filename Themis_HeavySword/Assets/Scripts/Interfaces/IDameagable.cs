using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDameagable
{
    public float TotalHealth { get; }
    public float CurrentHealth { get; }

    public bool Damage(float amount, GameObject attacker, bool overrideImmuneTime);
}
