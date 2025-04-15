using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTanks : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _oxygens;
    [SerializeField] private Color32 _oxygenColor;
    private float _oxygen = 15;

    public void SetOxygen(float oxygen)
    {
        _oxygen = oxygen;
        FillTankVisual();
    }

    public void FillTankVisual()
    {
        int tanksFilled = Mathf.FloorToInt(_oxygen / 5);
        for (int i = 0; i < tanksFilled; i++)
        {
            _oxygens[i].color = _oxygenColor;
        }
        if (tanksFilled >= _oxygens.Length)
            return;
        _oxygens[tanksFilled].color = new Color32(_oxygenColor.r, _oxygenColor.g, _oxygenColor.b, (byte)(((_oxygen / 5) - tanksFilled) * 255));
    }

    public void FillUpTank(float oxygen)
    {
        _oxygen += oxygen;
        FillTankVisual();
    }
}
