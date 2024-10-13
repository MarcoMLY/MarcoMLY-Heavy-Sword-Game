using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Regeneration : MonoBehaviour
{
    [SerializeField] private float _regenerationWait, _regenerationAmount, _regenerateSmallWait;
    private float _regenerationTimer;

    [SerializeField] private UnityEventFloat _aboutToRegenerate;
    [SerializeField] private UnityEventFloat _onRegenerate;

    private void Awake()
    {
        _regenerationTimer = _regenerationWait;
        StopAllCoroutines();
    }

    private void Update()
    {
        _regenerationTimer -= Time.deltaTime;
        if (_regenerationTimer <= 0)
        {
            _regenerationTimer = _regenerationWait;
            _aboutToRegenerate?.Invoke(_regenerationAmount);
            StartCoroutine(Regenerate());
        }
    }

    public void ResetRegeneration()
    {
        _regenerationTimer = _regenerationWait;
    }

    private IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(_regenerateSmallWait);
        _onRegenerate?.Invoke(_regenerationAmount);
    }
}
