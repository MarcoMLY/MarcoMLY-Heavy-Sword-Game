using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;

public class CrystalHealth : Health
{
    public override bool Damage(float damage, GameObject attacker, bool overrideImmuneTime)
    {
        return base.Damage(damage, attacker, overrideImmuneTime);
    }
}
