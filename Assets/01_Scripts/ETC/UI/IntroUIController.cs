using System;
using UnityEngine;
using UnityEngine.UIElements;

public class IntroUIController : MonoBehaviour{
    private UIDocument _document;
    private GameObject _settingDocumentObj;

    private bool isSettingOn = false;
    public bool IsSettingOn
    {
        get { return isSettingOn; }
        set 
        {
            isSettingOn = value;
            ShowSettingPanel(value);
        }
    }
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void Start()
    {
        _settingDocumentObj = transform.parent.Find("SettingUI").gameObject;
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
        IsSettingOn = !IsSettingOn;
    }

    private void ShowSettingPanel(bool value)
    {
        if(value)
        {
            _settingDocumentObj.SetActive(true);
        }
        else
        {
            _settingDocumentObj.SetActive(false);
        }
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
