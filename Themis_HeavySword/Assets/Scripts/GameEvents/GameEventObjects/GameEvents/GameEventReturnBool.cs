using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameEvents/GameEventReturnBool")]
public class GameEventReturnBool : ScriptableObject
{
    public delegate bool SendMessageDelegate();
    public event SendMessageDelegate SendMessage;

    public bool RecieveMessages()
    {
        if (SendMessage == null)
            return false;
        return SendMessage.Invoke();
    }

    public bool EventNull()
    {
        if (SendMessage == null)
            return true;
        return false;
    }
}
