using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHealthSlider : MonoBehaviour
{
    [SerializeField] public Health EnemyHealth;
    [SerializeField] private Transform _fillTransform;

    [SerializeField] private Gradient _colorGradient;
    [SerializeField] private SpriteRenderer _spritRenderer;

    [SerializeField] private float _lerpSpeed, _scaleInTime, _size, _disableTime;
    [SerializeField] private AnimationCurve _scaleIn;

    [SerializeField] private Vector3 _offset;

    private bool _notVisible = true;
    public bool Disabled { get; private set; } = true;

    private void Awake()
    {
        _notVisible = true;
        transform.localScale = new Vector2(_size, 0);
    }

    public void EnableSlider()
    {
        _notVisible = false;
        Disabled = false;
        if (EnemyHealth.CurrentHealth <= 0)
        {
            transform.localScale = new Vector2(_size, _size);
            return;
        }
        StartCoroutine(ScaleIn());
    }

    public void DisableSlider(float time)
    {
        StartCoroutine(DisableSliderAnimation(time));
    }

    private void Update()
    {
        transform.position = EnemyHealth.transform.position + _offset;
        if (_notVisible && EnemyHealth.TotalHealth > EnemyHealth.CurrentHealth)
        {
            EnableSlider();
        }
        if (Disabled)
            return;
        float value = Mathf.Clamp(EnemyHealth.CurrentHealth / EnemyHealth.TotalHealth, 0, 1);
        _fillTransform.localScale = new Vector2(_fillTransform.localScale.x, Mathf.Lerp(_fillTransform.localScale.y, value, Time.deltaTime * _lerpSpeed));

        _spritRenderer.color = Color.Lerp(_spritRenderer.color, _colorGradient.Evaluate(value), Time.deltaTime * _lerpSpeed);
    }

    private IEnumerator ScaleIn()
    {
        float scaleIn = 0;
        while (scaleIn <= 1)
        {
            scaleIn += Time.deltaTime / _scaleInTime;
            transform.localScale = new Vector2(_size, _scaleIn.Evaluate(scaleIn) * _size);
            yield return null;
        }
    }

    private IEnumerator DisableSliderAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        float scaleIn = 1;
        while (scaleIn > 0)
        {
            scaleIn -= Time.deltaTime / _scaleInTime;
            transform.localScale = new Vector2(_size, _scaleIn.Evaluate(scaleIn) * _size);
            yield return null;
        }

        transform.localScale = new Vector2(_size, 0);
        Destroy(gameObject);
    }

    public void EnemyDied()
    {
        transform.parent = null;
        StartCoroutine(DisableSliderAnimation(_disableTime));
    }
}
