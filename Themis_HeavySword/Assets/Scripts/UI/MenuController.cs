using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject[] _menus;

    public void SwitchToMenu(int menu)
    {
        for (int i = 0; i < _menus.Length; i++)
        {
            _menus[i].SetActive(i == menu);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
