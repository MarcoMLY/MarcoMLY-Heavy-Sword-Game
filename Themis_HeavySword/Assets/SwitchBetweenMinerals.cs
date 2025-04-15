using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBetweenMinerals : MonoBehaviour
{
    [SerializeField] private MaterialAtBase[] _mineralsAtBase;
    [SerializeField] private DayDataHolder _daySave;

    [SerializeField] private float _moveAmount;

    private List<MaterialAtBase> _enabledMinerals = new List<MaterialAtBase>();
    private MaterialAtBase _currentMineralSelected;
    private int _currentIndexSelected;

    [SerializeField] private GameObject _left;
    [SerializeField] private GameObject _right;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < _mineralsAtBase.Length; i++)
        {
            int amountOfMineral = _daySave.Day.MaterialsAndIndexes[_mineralsAtBase[i].MaterialType.Index];
            if (amountOfMineral > 0)
            {
                _enabledMinerals.Add(_mineralsAtBase[i]);
                _mineralsAtBase[i].gameObject.SetActive(true);
                _mineralsAtBase[i].transform.position = new Vector3(transform.position.x + (_moveAmount * i), transform.position.y, transform.position.z);
            }
        }

        if (_enabledMinerals.Count <= 0)
        {
            _currentMineralSelected = null;
            return;
        }
        _currentMineralSelected = _enabledMinerals[0];
        _currentIndexSelected = 0;
        DisableButtons();
    }

    public void MoveToLeft()
    {
        _currentIndexSelected -= 1;
        _currentMineralSelected = _enabledMinerals[_currentIndexSelected];
        DisableButtons();
        MoveMinerals(1);
    }

    public void MoveToRight()
    {
        _currentIndexSelected += 1;
        _currentMineralSelected = _enabledMinerals[_currentIndexSelected];
        DisableButtons();
        MoveMinerals(-1);
    }

    private void MoveMinerals(int direction)
    {
        for (int i = 0; i < _enabledMinerals.Count; i++)
        {
            _enabledMinerals[i].transform.position = new Vector3(transform.position.x + (_moveAmount * (i - _currentIndexSelected)), transform.position.y, transform.position.z);
            //if (_currentIndexSelected == i)
            //{
            //    //_enabledMinerals[i].transform.localScale = new Vector3();
            //}
        }
    }

    private void DisableButtons()
    {
        if (_currentIndexSelected <= 0)
            _left.gameObject.SetActive(false);
        if (_currentIndexSelected >= _enabledMinerals.Count - 1)
            _right.gameObject.SetActive(false);
        if (_currentIndexSelected <= _enabledMinerals.Count - 1 && _currentIndexSelected > 0)
            _left.gameObject.SetActive(true);
        if (_currentIndexSelected >= 0 && _currentIndexSelected < _enabledMinerals.Count - 1)
            _right.gameObject.SetActive(true);
    }
}
