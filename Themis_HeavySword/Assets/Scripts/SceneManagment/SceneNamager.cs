using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneNamager : DontDestroyOnLoadScene<SceneNamager>
{
    public void OpenScene(int index)
    {
        StartCoroutine(LoadNextFrame(index));
    }

    private IEnumerator LoadNextFrame(int index)
    {
        yield return null;
        SceneManager.LoadSceneAsync(index);
    }
}
