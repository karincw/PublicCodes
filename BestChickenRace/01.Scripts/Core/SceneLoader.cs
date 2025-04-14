using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance = null;

    public void LoadSceneAsync(string sceneName, Action onCompleted = null)
    {
        StartCoroutine(LoadSceneCorutine(sceneName, onCompleted));
    }

    private IEnumerator LoadSceneCorutine(string sceneName, Action onCompleted)
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);

        while (true)
        {
            if (oper.isDone)
                break;

            yield return new WaitForSeconds(0.1f);
        }

        onCompleted?.Invoke();
    }
}
