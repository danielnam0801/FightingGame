using System;
using UnityEngine;
using UnityEngine.UIElements;

public class IntroUIController : MonoBehaviour{
    private UIDocument _document;
    private GameObject _settingDocumentObj;

    bool isSettingOn = false;
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void Start()
    {
        _settingDocumentObj = transform.Find("SettingUI").gameObject;
    }

    private void OnEnable()
    {
        var root = _document.rootVisualElement;

        Button startBtn = root.Q<Button>("StartBtn");
        Button settingBtn = root.Q<Button>("SettingBtn");
        Button quitBtn = root.Q<Button>("QuitBtn");
        
        startBtn.RegisterCallback<ClickEvent>(GameStart);
        settingBtn.RegisterCallback<ClickEvent>(ActiveSetting);
        quitBtn.RegisterCallback<ClickEvent>(Quit);
    }

    private void ActiveSetting(ClickEvent evt)
    {
        isSettingOn = !isSettingOn;
        
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
        SceneManager.Instance.LoadSelectScene();
    }

}
