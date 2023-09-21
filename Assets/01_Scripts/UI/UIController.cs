using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour{
    private UIDocument _document;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _document.rootVisualElement;

        Button startBtn = root.Q<Button>("StartBtn");
        Button quitBtn = root.Q<Button>("QuitBtn");
        
        startBtn.RegisterCallback<ClickEvent>(GameStart);
        quitBtn.RegisterCallback<ClickEvent>(Quit);
    }

    private void Quit(ClickEvent evt)
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void GameStart(ClickEvent evt)
    {
        SceneManager.LoadScene("");
    }

}
