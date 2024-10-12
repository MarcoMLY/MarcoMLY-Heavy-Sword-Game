using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Beam : MonoBehaviour
{
    [SerializeField] private float _maxDistance, _duration = 10f, _damage = 1f, _hitboxWidth = 0.1f;
    [SerializeField] LayerMask _passThrough, _blockBeam;
    [SerializeField] private UnityEvent _onSpawn, _onDestroy;
    private LineRenderer _lineRenderer;
    private SpriteRenderer _spriteRenderer;
    private float _distance;

    private bool _enabled = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _lineRenderer.enabled = false;
        _spriteRenderer.enabled = false;
        _enabled = false;
    }

    public void SpawnProjectile()
    {
        StartCoroutine(DestroyTimer());
        _onSpawn?.Invoke();

        CheckDistance();
        Visual();

        _lineRenderer.enabled = true;
        _spriteRenderer.enabled = true;
        _enabled = true;
    }

    private void CheckDistance()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, _maxDistance, _blockBeam);
        _distance = hit.distance;
        if (hit.collider == null)
            _distance = _maxDistance;
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(_duration);
        DestroyProjectile();
    }

    private void Visual()
    {
        Vector3[] points = { transform.position, transform.position + (transform.up * _distance) };
        _lineRenderer.SetPositions(points);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enabled)
            return;
        CheckDistance();
        Visual();

        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, _hitboxWidth / 2, transform.up, _distance);

        foreach (RaycastHit2D obsticle in hit)
        {
            if (_passThrough.Contains(obsticle.collider.gameObject.layer))
                continue;
            IDameagable damageable = obsticle.collider.gameObject.GetComponent<IDameagable>();
            if (damageable != null && _damage > 0)
            {
                damageable.Damage(_damage, gameObject, false);
            }
        }
    }

    private void DestroyProjectile()
    {
        _onDestroy?.Invoke();
        _lineRenderer.enabled = false;
        _spriteRenderer.enabled = false;
        _enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 widthDirection = (transform.right * (_hitboxWidth / 2));
        Gizmos.DrawLine(transform.position + widthDirection, transform.position + (transform.up * _maxDistance) + widthDirection);
        Gizmos.DrawLine(transform.position - widthDirection, transform.position + (transform.up * _maxDistance) - widthDirection);
    }
}
