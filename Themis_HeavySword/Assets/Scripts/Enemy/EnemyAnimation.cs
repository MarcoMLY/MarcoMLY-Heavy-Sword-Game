using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private float _moveUpAmount, _moveTime;
    [SerializeField] private Transform _enemySprite;

    public void StartMoveUp()
    {
        StopAllCoroutines();
        StartCoroutine(MoveUp());
    }

    public void StartMoveDown()
    {
        StopAllCoroutines();
        StartCoroutine(MoveDown());
    }

    private IEnumerator MoveUp()
    {
        Vector2 upPosition = new Vector2(0, _moveUpAmount);
        while (_enemySprite.localPosition.y < (_moveUpAmount - 0.01f))
        {
            _enemySprite.localPosition = Vector2.Lerp(_enemySprite.localPosition, upPosition, Time.deltaTime / _moveTime);
            yield return null;
        }

        _enemySprite.localPosition = upPosition;
    }

    private IEnumerator MoveDown()
    {
        Vector2 downPosition = Vector2.zero;
        while (_enemySprite.localPosition.y > 0.01f)
        {

            _enemySprite.localPosition = Vector2.Lerp(_enemySprite.localPosition, Vector2.zero, Time.deltaTime / _moveTime);
            yield return null;
        }

        _enemySprite.localPosition = downPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + _moveUpAmount));
    }
}
