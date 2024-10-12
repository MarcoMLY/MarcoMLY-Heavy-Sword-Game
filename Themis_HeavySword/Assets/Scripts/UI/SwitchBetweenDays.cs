using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Data;
using TMPro;
using System.Security.Cryptography;

public class SwitchBetweenDays : MonoBehaviour
{
    [SerializeField] private IntListHolder _saveSlotDays;
    [SerializeField] private IntHolder _saveSlot;
    [SerializeField] private TransformHolder _camera;
    private Camera _mainCam;

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private RectTransform _dayNumbersPivot;
    private List<TextMeshProUGUI> _texts = new List<TextMeshProUGUI>();

    [SerializeField] private float _mousePullMultiplier, _distanceBetweenNumbers, _lerpToDaySpeed, _lerpNumberSizeTime, _clickRadius, _numberNormalSize, _numberBiggerSize;

    private int _day, _maxDay, _scrollStartDay;
    private float _currentDay;

    public int Day { get => _day; }

    private float _mouseXStart;
    private bool _checkingMouseDistance;

    private void OnEnable()
    {
        _maxDay = _saveSlotDays.Variable[_saveSlot.Variable];
        _mainCam = _camera.Variable.gameObject.GetComponent<Camera>();
        _texts.Add(_text);
        _day = _maxDay;
        _currentDay = _day;
        _dayNumbersPivot.position = new Vector3(transform.position.x - (_distanceBetweenNumbers * _day), transform.position.y, transform.position.z);
        CreateNumbers();
    }

    private void OnDisable()
    {
        DisableNumbers();
    }

    private void DisableNumbers()
    {
        if (_texts.Count <= 0)
            return;
        for (int i = 1; i < _texts.Count; i++)
        {
            _texts[i].gameObject.SetActive(false);
        }
    }

    private void CreateNumbers()
    {
        for (int i = 1; i <= _maxDay; i++)
        {
            TextMeshProUGUI newNumber = null;
            if (_texts.Count > i)
            {
                newNumber = _texts[i];
                _texts[i].gameObject.SetActive(true);
            }
            if (_texts.Count <= i)
            {
                Vector3 position = new Vector3(_text.rectTransform.position.x + (_distanceBetweenNumbers * i), _text.rectTransform.position.y, transform.position.z);
                newNumber = Instantiate(_text.gameObject, position, Quaternion.identity, _dayNumbersPivot).GetComponent<TextMeshProUGUI>();
                _texts.Add(newNumber);
            }
            newNumber.text = i.ToString();
        }
    }

    private void Update()
    {
        DragToDayCheck();
        LerpToBiggerNumber();
    }

    private void LerpToBiggerNumber()
    {
        for (int i = 0; i < _texts.Count; i++)
        {
            float newScale = i == _day ? _numberBiggerSize : _numberNormalSize;
            _texts[i].transform.localScale = Vector2.Lerp(_texts[i].transform.localScale, new Vector2(newScale, newScale), Time.deltaTime / _lerpNumberSizeTime);
        }
    }

    private bool CheckIfMouseOver()
    {
        float mousePosY = _mainCam.ScreenToWorldPoint(Input.mousePosition).y;
        return mousePosY <= transform.position.y + _clickRadius && mousePosY >= transform.position.y - _clickRadius;
    }

    private void DragToDayCheck()
    {
        _day = Mathf.RoundToInt(_currentDay);
        BoundCheckDay();
        if (!_checkingMouseDistance)
            return;
        float mouseX = _mainCam.ScreenToWorldPoint(Input.mousePosition).x;
        float distanceInDays = mouseX - _mouseXStart;
        _currentDay = _scrollStartDay - (distanceInDays / _distanceBetweenNumbers);
        BoundCheckDay();
        Vector3 newPivotPos = new Vector3(transform.position.x - (_scrollStartDay * _distanceBetweenNumbers) + distanceInDays, transform.position.y, transform.position.z);
        _dayNumbersPivot.position = Vector2.Lerp(_dayNumbersPivot.position, newPivotPos, Time.deltaTime * _lerpToDaySpeed);
    }

    private IEnumerator LerpToClosestDay()
    {
        Vector3 newPivotPosition = new Vector3(transform.position.x - (_distanceBetweenNumbers * _day), transform.position.y, transform.position.z);
        while (Vector2.Distance(_dayNumbersPivot.position, newPivotPosition) > 0.05f)
        {
            _dayNumbersPivot.position = Vector2.Lerp(_dayNumbersPivot.position, newPivotPosition, Time.deltaTime * _lerpToDaySpeed);
            yield return null;
        }
        _dayNumbersPivot.position = newPivotPosition;
    }

    public void OnScroll(float mouseScroll)
    {
        if (_checkingMouseDistance)
            return;
        if (!CheckIfMouseOver())
            return;
        if (mouseScroll > 0)
            _currentDay += 1;
        if (mouseScroll < 0)
            _currentDay -= 1;
        _day = Mathf.RoundToInt(_currentDay);
        BoundCheckDay();
        _currentDay = _day;
        _dayNumbersPivot.position = Vector3.Lerp(_dayNumbersPivot.position, new Vector3(transform.position.x - (_day * _distanceBetweenNumbers), transform.position.y, 0), Time.deltaTime * _lerpToDaySpeed);
    }

    public void OnClick()
    {
        if (_checkingMouseDistance)
            return;
        if (!CheckIfMouseOver())
            return;
        StopAllCoroutines();
        _checkingMouseDistance = true;
        _mouseXStart = _mainCam.ScreenToWorldPoint(Input.mousePosition).x;
        _scrollStartDay = _day;
    }

    public void OnUnclick()
    {
        if (!_checkingMouseDistance)
            return;
        BoundCheckDay();
        StartCoroutine(LerpToClosestDay());
        _checkingMouseDistance = false;
    }

    private void BoundCheckDay()
    {
        if (_day > _maxDay)
            _day = _maxDay;
        if (_day < 0)
            _day = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(transform.position.x - 50, transform.position.y + _clickRadius, transform.position.z), new Vector3(transform.position.x + 50, transform.position.y + _clickRadius, transform.position.z));
        Gizmos.DrawLine(new Vector3(transform.position.x - 50, transform.position.y - _clickRadius, transform.position.z), new Vector3(transform.position.x + 50, transform.position.y - _clickRadius, transform.position.z));
    }
}
