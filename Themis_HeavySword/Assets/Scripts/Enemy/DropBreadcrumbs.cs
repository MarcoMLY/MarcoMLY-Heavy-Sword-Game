using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;

public class DropBreadcrumbs : MonoBehaviour
{
    [SerializeField] private Transform _breadcrumb;
    private List<Transform> _breadcrumbs = new List<Transform>();
    [SerializeField] private TransformListHolder _breadcrumbsHolder;
    [SerializeField] private TransformHolder _player;
    [SerializeField] private IntHolder _newestBreadcrumbIndex;
    [SerializeField] private float _distanceBetweenBreadcrumb;
    [SerializeField] private int _breadcrumbsInScene;
    private int _currentBreadcrumb = 0;

    private void Start()
    {
        SpawnBreadcrumbs();
    }

    private void OnDisable()
    {
        _breadcrumbsHolder.ClearData();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.Variable == null)
            return;
        int previusBreadcrumb = _currentBreadcrumb - 1;
        if (previusBreadcrumb < 0)
            previusBreadcrumb = _breadcrumbsInScene - 1;
        float distance = Vector2.Distance(_player.Variable.position, _breadcrumbs[previusBreadcrumb].position);
        if (distance >= _distanceBetweenBreadcrumb)
            MoveBreadcrumb();
    }

    private void SpawnBreadcrumbs()
    {
        for (int i = 0; i < _breadcrumbsInScene; i++)
        {
            Transform breadCrumb = CreateBreadcrumb();
            _breadcrumbsHolder.AddData(breadCrumb);
            _breadcrumbs.Add(breadCrumb);
        }
    }

    private void MoveBreadcrumb()
    {
        GetBreadcrumb(_breadcrumbs[_currentBreadcrumb]);
        _newestBreadcrumbIndex.ChangeData(_currentBreadcrumb);

        _currentBreadcrumb += 1;
        if (_currentBreadcrumb >= _breadcrumbsInScene)
            _currentBreadcrumb = 0;
    }

    private Transform CreateBreadcrumb()
    {
        Transform newBreadcrumb = Instantiate(_breadcrumb, _player.Variable.position, Quaternion.identity, transform).transform;
        return newBreadcrumb;
    }

    private void GetBreadcrumb(Transform breadcrumb)
    {
        breadcrumb.gameObject.SetActive(true);
        breadcrumb.position = _player.Variable.position;
        //Debug.Log(breadcrumb.position == _player.Variable.position);
    }

    private void DisableBreadcrumb(Transform breadcrumb)
    {
        breadcrumb.gameObject.SetActive(false);
    }

    private void DestroyBreadcrumb(Transform breadcrumb)
    {
        Destroy(breadcrumb);
    }
}
