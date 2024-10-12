using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFrameRate : MonoBehaviour
{
    [SerializeField] private int _frameRateDecreaseAmount = 0;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _frameRateDecreaseAmount * _frameRateDecreaseAmount; i++)
        {
            GameObject _o = FindObjectsByType<PlayerMove>(FindObjectsSortMode.None)[0].gameObject;
        }
    }
}
