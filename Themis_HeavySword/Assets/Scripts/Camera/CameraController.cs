using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;
using Cinemachine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private TransformHolder _player, _sword;

    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _mouseOffsetMultiplier = 2f, _smoothness = 0.1f, _zoomSmoothness = 0.1f, _minZoom = 10f, _maxZoom = 40f;

    private float minDistance = 3f;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player.Variable == null || _sword.Variable == null)
            return;
        Move();
        Zoom();
    }

    private void Move()
    {
        Vector2 playerPos = _player.Variable.position;
        Vector2 swordPos = _sword.Variable.position;

        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 directionToMouse = (mousePos - playerPos).normalized;
        Vector2 offsetedPosition = playerPos + (directionToMouse * _mouseOffsetMultiplier);
        Vector2 playerAndSwordMid = playerPos;
        if (Vector2.Distance(playerPos, swordPos) > minDistance)
        {
            Bounds boundingBox = new Bounds(playerPos, Vector3.zero);
            boundingBox.Encapsulate(swordPos);

            playerAndSwordMid = boundingBox.center;
        }

        Vector2 finalPos = playerAndSwordMid + (directionToMouse * _mouseOffsetMultiplier);
        transform.position = Vector3.Slerp(transform.position, (Vector3)finalPos + _offset, Time.deltaTime / _smoothness);
    }

    private void Zoom()
    {
        Vector2 playerPos = _player.Variable.position;
        Vector2 swordPos = _sword.Variable.position;

        float distance = Mathf.Clamp(Vector2.Distance(playerPos, swordPos), minDistance, Mathf.Infinity);
        float zoomLimiter = 100f;
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, Mathf.Lerp(_minZoom, _maxZoom, distance / zoomLimiter), Time.deltaTime / _zoomSmoothness);
    }
}
