using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateable
{
    public bool Enabled { get; set; }
    public State HandleState();
    public void OnStateEnabled();
}
