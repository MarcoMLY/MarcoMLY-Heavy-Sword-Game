using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private AnimationCurve _damageTextScale;
    [SerializeField] private AnimationCurve _turnAmount;
    [SerializeField] private AnimationCurve _moveUpAmount;

    [SerializeField] private float _textSizeMultiplier;
    [SerializeField] private float _textCriticalStrikeSizeMultiplier;
    [SerializeField] private float _moveUpMultiplier;

    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _criticalStrikeColor;

    [SerializeField] private float _randomSizeAmount;
    [SerializeField] private float _randomTurnMultiplier;
    [SerializeField] private float _randomMoveAmount;

    [SerializeField] private float _randomOffsetAmount;
    [SerializeField] private float _time;

    [SerializeField] private string _prefix;

    private TextMeshPro _text;

    public void SpawnDamageText(float damage, bool criticalStrike)
    {
        _text = GetComponent<TextMeshPro>();
        transform.localScale = Vector2.zero;

        transform.position += new Vector3(Random.Range(-_randomOffsetAmount, _randomOffsetAmount), Random.Range(-_randomOffsetAmount, _randomOffsetAmount), 0);

        _text.text = _prefix + damage.ToString();
        _text.color = criticalStrike ? _criticalStrikeColor : _normalColor;

        float sizeMultiplier = criticalStrike ? _textCriticalStrikeSizeMultiplier : _textSizeMultiplier;
        sizeMultiplier += Random.Range(sizeMultiplier * -_randomSizeAmount, sizeMultiplier * _randomSizeAmount);

        float turnAmount = Random.Range(-_randomTurnMultiplier, _randomTurnMultiplier);
        float moveUpAmount = Random.Range(_moveUpMultiplier - _randomMoveAmount, _moveUpMultiplier + _randomMoveAmount);
        StartCoroutine(DamageTextAnimation(sizeMultiplier, turnAmount, moveUpAmount));
    }

    private IEnumerator DamageTextAnimation(float sizeMultiplier, float turnAmount, float moveUpAmount)
    {
        float progress = 0f;
        while (progress < _damageTextScale.length)
        {
            progress += Time.deltaTime / _time;
            transform.localScale = new Vector2(_damageTextScale.Evaluate(progress), _damageTextScale.Evaluate(progress)) * sizeMultiplier;
            transform.Rotate(new Vector3(0, 0, _turnAmount.Evaluate(progress) * turnAmount));
            transform.position += Vector3.up * _moveUpAmount.Evaluate(progress) * moveUpAmount;
            yield return null;
        }

        Destroy(gameObject);
    }
}
