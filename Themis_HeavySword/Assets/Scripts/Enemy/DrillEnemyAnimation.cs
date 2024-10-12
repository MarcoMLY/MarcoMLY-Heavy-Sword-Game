using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DrillEnemyAnimation : MonoBehaviour
{
    [SerializeField] private Transform _enemySprite, _enemyShadow;
    [SerializeField] private float _timeBetweenCrack, _maxSideDisplacement;
    [SerializeField] private int _lineLength;
    [SerializeField] private LineRenderer _lineRenderer;
    private TimerManager _timer;
    private bool _left;

    private void Awake()
    {
        _timer = GetComponent<TimerManager>();
        Vector3[] points = new Vector3[_lineLength];
        for (int i = 0; i < _lineLength; i++)
        {
            points[i] = transform.position;
        }
        _lineRenderer.positionCount = _lineLength;
        _lineRenderer.SetPositions(points);

        _left = Random.Range(0, 2) == 0;

        StartCoroutine(DrillLine());
    }

    public void GoIntoGround()
    {
        _enemySprite.gameObject.SetActive(false);
        _enemyShadow.gameObject.SetActive(false);
    }

    public void LeaveGround()
    {
        _enemySprite.gameObject.SetActive(true);
        _enemyShadow.gameObject.SetActive(true);
    }

    private void Update()
    {
        //if (!_timer.FindTimer(PlacePoint))
        //{
        //    _timer.StartTimer(_timeBetweenCrack, PlacePoint);
        //    Debug.Log(Time.time);
        //}
    }

    private IEnumerator DrillLine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeBetweenCrack);
            PlacePoint();
        }

        //while (_inGround)
        //{
        //    yield return new WaitForSeconds(_timeBetweenCrack);
        //    PlacePoint();
        //}

        //while (!_inGround)
        //{
        //    yield return null;
        //}

        //StartCoroutine(DrillLine());
    }

    private void PlacePoint()
    {
        Vector3 point = transform.position;
        point += _enemySprite.right.normalized * (_maxSideDisplacement * Random.Range(0.10f, 1.00f)) * (_left ? -1 : 1);
        for (int i = _lineRenderer.positionCount - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                _lineRenderer.SetPosition(i, point);
                continue;
            }
            _lineRenderer.SetPosition(i, _lineRenderer.GetPosition(i - 1));
        }

        if (_left)
        {
            _left = false;
            return;
        }
        _left = true;
    }
}
