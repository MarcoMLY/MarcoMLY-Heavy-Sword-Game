using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 3f, _duration = 10f, _damage = 1f;
    [SerializeField] Vector2 _hitbox;
    [SerializeField] LayerMask _passThrough;
    [SerializeField] private UnityEvent _onSpawn, _onDestroy;
    //private TimerManager _timer;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(DestroyTimer());
        _onSpawn?.Invoke();
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(_duration);
        DestroyProjectile();
    }

    // Update is called once per frame
    void Update()
    {
        MoveFoward();

        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, _hitbox, transform.rotation.z);

        foreach(Collider2D obsticle in hit)
        {
            IDameagable damageable = obsticle.gameObject.GetComponent<IDameagable>();
            if (damageable != null && _damage > 0)
            {
                damageable.Damage(_damage, gameObject, false);
            }

            if (!_passThrough.Contains(obsticle.gameObject.layer))
            {
                DestroyProjectile();
            }
        }
    }

    private void MoveFoward()
    {
        Vector3 fowardDirection = transform.up * _speed * Time.deltaTime;
        transform.position += fowardDirection;
    }

    private void DestroyProjectile()
    {
        _onDestroy?.Invoke();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _hitbox);
    }
}
