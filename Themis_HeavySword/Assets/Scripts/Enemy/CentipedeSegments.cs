using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeSegments : MonoBehaviour
{
    [SerializeField] private Transform _segment, _head;
    [SerializeField] private int _length;
    [SerializeField] private float _smoothTime, _trailSpeed, _turnSpeed, _segmentDistance;

    private Vector3 _pastHeadPosition;
    private Transform[] _segments;
    private Vector3[] _segmentVelocities;

    private void Awake()
    {
        _segments = new Transform[_length];
        _segmentVelocities = new Vector3[_length];
        _pastHeadPosition = _head.position;
        _segments[0] = _head;
        for (int i = 1; i < _length; i++)
        {
            _segments[i] = Instantiate(_segment, transform.position - (_head.up * _segmentDistance * i), _head.rotation, transform);
        }
    }

    private void Update()
    {
        if (_head == null)
            return;
        //if (_pastHeadPosition == _head.position)
            //return;
        for (int i = 1; i < _length; i++)
        {
            //move
            _segments[i].position = Vector3.SmoothDamp(_segments[i].position, _segments[i - 1].position - (_segmentVelocities[i - 1].normalized * _segmentDistance), ref _segmentVelocities[i], _smoothTime + i / _trailSpeed);
            //turn
            Vector3 _segmentDirection = (_segments[i].position - _segments[i - 1].position).normalized;
            _segments[i].up = -_segmentDirection;
            //stay at distance
            _segments[i].position = _segments[i - 1].position + (_segmentDirection * _segmentDistance);
        }
        _pastHeadPosition = _head.position;
    }
}
