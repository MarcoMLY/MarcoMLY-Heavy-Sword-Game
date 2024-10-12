using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSomething : MonoBehaviour
{
    [SerializeField] protected Vector2 _hitBox;
    [SerializeField] protected Vector2 _offset;
    [SerializeField] protected LayerMask _layer;

    [SerializeField] protected float _damage = 1f;

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckCollider();
    }

    protected virtual void CheckCollider()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position + (Vector3)((Vector2)transform.up * _offset.y) + (Vector3)((Vector2)transform.right * _offset.x), _hitBox, transform.rotation.eulerAngles.z, _layer);

        foreach (Collider2D collidedObject in hit)
        {
            IDameagable damageable = collidedObject.GetComponent<IDameagable>();

            if (damageable != null)
            {
                damageable.Damage(_damage, gameObject, false);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)((Vector2)transform.up * _offset.y) + (Vector3)((Vector2)transform.right * _offset.x), _hitBox);
    }
}
