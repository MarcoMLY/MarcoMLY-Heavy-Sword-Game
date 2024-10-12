using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffects : MonoBehaviour
{
    private SwordDamage _swordDamage;
    [SerializeField] private GameObject _criticalStrikeEffect;
    [SerializeField] private GameObject _damageText;

    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _criticalStrikeColor;

    // Start is called before the first frame update
    void Awake()
    {
        _swordDamage = GetComponent<SwordDamage>();
    }

    private void OnEnable()
    {
        _swordDamage.OnCriticalStrike += DamageEffect;
    }

    private void OnDisable()
    {
        _swordDamage.OnCriticalStrike -= DamageEffect;
    }

    public void DamageEffect(bool criticalStrike, float damage)
    {
        DamageText damageText = Instantiate(_damageText, transform.position, Quaternion.identity).GetComponent<DamageText>();
        damageText.SpawnDamageText(damage, criticalStrike);
        if (!criticalStrike)
            return;
        Transform effect = Instantiate(_criticalStrikeEffect, transform.position, Quaternion.identity).transform;
        effect.up = -transform.up;
    }
}
