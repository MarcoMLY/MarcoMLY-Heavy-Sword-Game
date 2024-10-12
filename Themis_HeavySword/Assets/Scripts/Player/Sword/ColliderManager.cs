using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Data;

public class ColliderManager : MonoBehaviour
{
    [SerializeField] private GameObject _swordColliderFull;
    [SerializeField] private GameObject _swordColliderPassThroughWalls;
    [SerializeField] private LayerMaskHolder _obsticle;
    [SerializeField] private BoolHolder _playerSpinningSword;

    [SerializeField] private Transform[] _stuckChecks;
    private bool _collisionsDisabled;

    private void Awake()
    {
        SetCollisionFull();
    }

    private void Update()
    {
        bool stuckCheck = false;
        for (int i = 0; i < _stuckChecks.Length; i++)
        {
            Collider2D checkIfSwordGlitched1 = Physics2D.OverlapPoint(_stuckChecks[i].position, _obsticle.Variable);
            if (checkIfSwordGlitched1)
            {
                stuckCheck = true;
                break;
            }
        }
        if (_playerSpinningSword.Variable)
            return;
        if (stuckCheck)
        {
            SetCollisionPassThroughWalls();
            _collisionsDisabled = true;
            return;
        }
        if (_collisionsDisabled)
        {
            SetCollisionFull();
            _collisionsDisabled = false;
        }
    }

    private bool CollisionDisabled(GameObject collider)
    {
        if (!collider.activeInHierarchy)
            return true;
        return false;
    }

    public void DisableCollision()
    {
        _swordColliderFull.gameObject.SetActive(false);
        _swordColliderPassThroughWalls.gameObject.SetActive(false);
    }

    public void SetCollisionFull()
    {
        _swordColliderFull.gameObject.SetActive(true);
        _swordColliderPassThroughWalls.gameObject.SetActive(false);
    }

    public void SetCollisionPassThroughWalls()
    {
        _swordColliderFull.gameObject.SetActive(false);
        _swordColliderPassThroughWalls.gameObject.SetActive(true);
    }
}
