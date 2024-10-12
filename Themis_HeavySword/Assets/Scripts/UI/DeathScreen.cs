using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] _buttons;
    [SerializeField] private TextMeshProUGUI _titleText;

    [SerializeField] private float _fadeTime;
    [SerializeField] private float _waitToFade, _waitToAppearButtons;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;

        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].SetActive(false);
        }
    }

    public void PlayerLostoxygen()
    {
        _titleText.text = "YOU LOST OXYGEN";
        StartCoroutine(Fade(_canvasGroup.alpha, 1, _canvasGroup));
    }

    public void PlayerKilled()
    {
        _titleText.text = "YOU DIED";
        StartCoroutine(Fade(_canvasGroup.alpha, 1, _canvasGroup));
    }

    private IEnumerator Fade(float start, float end, CanvasGroup group)
    {
        yield return new WaitForSeconds(_waitToFade);

        float timer = 0;
        while (timer < _fadeTime)
        {
            timer += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, end, timer / _fadeTime);
            yield return null;
        }

        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;

        for (int i = 0; i < _buttons.Length; i++)
        {
            yield return new WaitForSeconds(_waitToAppearButtons);
            _buttons[i].SetActive(true);
        }
    }
}
