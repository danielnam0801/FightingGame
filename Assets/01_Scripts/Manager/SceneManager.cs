using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCor(sceneName));
    }

    IEnumerator LoadSceneCor(string name)
    {
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName: name);
        while(!async.isDone)
        {
            yield return null;
            Debug.Log(async.progress);
        }
        Debug.Log("¾À ·Îµå ¼º°ø");
    }

    public void LoadIntroScene() => LoadScene("IntroScene");
    public void LoadSelectScene() => LoadScene("SelectScene");
    public void LoadGameScene() => LoadScene("TrashMap");
}
