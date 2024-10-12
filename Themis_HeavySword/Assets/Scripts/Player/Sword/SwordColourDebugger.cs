using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;

public class SwordColourDebugger : MonoBehaviour
{
    private bool _canThrowSword = false;
    [SerializeField] private FloatHolder _swordSpeed;
    [SerializeField] private FloatConstantHolder _fastSwordSpeed, _superFastSwordSpeed;
    private StatesDebugger _colourChange;

    private void Awake()
    {
        _colourChange = GetComponent<StatesDebugger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canThrowSword)
        {
            _colourChange.ChangeColourState("NormalSpeed");
            return;
        }
        if (_swordSpeed.Variable >= _superFastSwordSpeed.Variable)
        {
            _colourChange.ChangeColourState("SuperFastSpeed");
            return;
        }
        if (_swordSpeed.Variable >= _fastSwordSpeed.Variable)
        {
            _colourChange.ChangeColourState("FastSpeed");
            return;
        }
    }

    public void CanOrCantThrowSword(bool canThrow)
    {
        _canThrowSword = canThrow;
    }
}
