using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class UIMove : MonoBehaviour
{
    [SerializeField] private Transform _moveToPos;
    [SerializeField] private Transform _moveFromPos;
    [SerializeField] private float _moveTime, _returnWaitTime;
    private float _returnWaitTimer;

    public void MoveUI()
    {
        if (_returnWaitTimer > 0)
        {
            StopAllCoroutines();
            _returnWaitTimer = 0;
            StartCoroutine(OnMove(false));
            return;
        }
        StartCoroutine(OnMove(true));
        _returnWaitTimer = _returnWaitTime;
    }

    private IEnumerator OnMove(bool moveTowards)
    {
        float time = moveTowards ? _moveTime : 0;
        while (time >= 0 && time <= _moveTime)
        {
            time -= Time.deltaTime * (moveTowards ? 1 : -1);
            transform.localPosition = Vector3.Slerp(_moveToPos.localPosition, _moveFromPos.localPosition, time / _moveTime);
            yield return null;
        }
        transform.localPosition = moveTowards ? _moveToPos.localPosition : _moveFromPos.localPosition;
    }

    private void Update()
    {
        if (_returnWaitTimer <= 0)
            return;
        _returnWaitTimer -= Time.deltaTime;
        if (_returnWaitTimer <= 0)
            StartCoroutine(OnMove(false));
    }
}
