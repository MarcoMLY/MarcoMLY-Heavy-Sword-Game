using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void TriggerTrigger(string triggerName)
    {
        _anim.SetTrigger(triggerName);
    }

    public void EnableBool(string boolName)
    {
        _anim.SetBool(boolName, true);
    }

    public void DisableBool(string boolName)
    {
        _anim.SetBool(boolName, true);
    }
}
