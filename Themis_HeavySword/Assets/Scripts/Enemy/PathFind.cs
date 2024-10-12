using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Data;

public class PathFind : MonoBehaviour
{
    [SerializeField] private TransformListHolder _breadcrumbs;
    [SerializeField] private TransformHolder _player;
    [SerializeField] private IntHolder _newestBreadcrumb;
    [SerializeField] private LayerMaskHolder _wall;
    [SerializeField] private float _changeTargetDistance, _radiusOfEnemy;
    private Vector3 _currentBreadcrumbPos;
    private bool _doesntHaveBreadcrumb = true;

    // Start is called before the first frame update
    void Start()
    {
        _currentBreadcrumbPos = transform.position;
    }

    private void Update()
    {
        if (_player.Variable == null)
            return;
        if (HasLineOfSight(_player.Variable, _radiusOfEnemy))
        {
            _doesntHaveBreadcrumb = true;
        }
    }

    public Vector2 FindPathToPlayer()
    {
        if (_player.Variable == null)
            return Vector2.zero;
        if (HasLineOfSight(_player.Variable, _radiusOfEnemy / 2))
        {
            Vector2 straightToPlayer = (_player.Variable.position - transform.position).normalized;
            return straightToPlayer;
        }

        float distanceToCurrent = Vector2.Distance(transform.position, _currentBreadcrumbPos);
        if (distanceToCurrent <= _changeTargetDistance || _doesntHaveBreadcrumb)
        {
            Transform newBreadcrumb = FindClosestLOSBreadcrumb();
            if (newBreadcrumb == null)
            {
                _doesntHaveBreadcrumb = true;
                return Vector2.zero;
            }
            _currentBreadcrumbPos = newBreadcrumb.position;
            _doesntHaveBreadcrumb = false;
        }

        Vector2 direction = (_currentBreadcrumbPos - transform.position).normalized;
        return direction;
    }

    private Transform FindClosestLOSBreadcrumb()
    {
        Transform bestBreadcrumb = null;

        int newestIndex = _newestBreadcrumb.Variable;
        int index = newestIndex;

        while (bestBreadcrumb == null)
        {
            Transform currentBreadCrumb = _breadcrumbs.Variable[index];
            if (HasLineOfSight(currentBreadCrumb, 0.2f))
            {
                bestBreadcrumb = currentBreadCrumb;
                continue;
            }

            index -= 1;
            if (index < 0)
                index = _breadcrumbs.Variable.Count - 1;
            if (index == newestIndex)
                break;
        }

        if (bestBreadcrumb != null)
            return bestBreadcrumb;
        return null;
    }

    protected bool HasLineOfSight(Transform point, float radius)
    {
        Vector2 rayDireciton = point.position - transform.position;
        float distance = Vector2.Distance(point.position, transform.position);
        RaycastHit2D[] ray = Physics2D.CircleCastAll(transform.position, radius, rayDireciton, distance, _wall.Variable);
        return ray.Length <= 0;
    }
}
