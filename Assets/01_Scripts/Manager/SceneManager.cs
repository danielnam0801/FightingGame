using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string name)
    {
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName: name);
        while(!async.isDone)
        {
            yield return null;
            Debug.Log(async.progress);
        }
        Debug.Log("¾À ·Îµå ¼º°ø");
    }
}
