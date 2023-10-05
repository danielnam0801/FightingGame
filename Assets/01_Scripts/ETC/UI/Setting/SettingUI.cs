using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingUI : MonoBehaviour
{
    private UIDocument uiDocument;
    private IntroUIController controller;
    [SerializeField] VisualTreeAsset soundSettingAsset;
    List<SoundSetting> soundSettings = new List<SoundSetting>();


    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        controller = transform.parent.Find("IntroUI").GetComponent<IntroUIController>();
    }

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;

        MakeSoundSettings();
        Button escButton = root.Q<Button>("ESC");
        escButton.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("CLick");
            controller.IsSettingOn = false;
        });

    }

    private void MakeSoundSettings()
    {
        VisualElement soundSettingContainer = uiDocument.rootVisualElement.Q<ScrollView>("SoundSettingContainer").contentContainer;
        soundSettingContainer.style.alignItems = Align.Center;
        for(int i = 0; i < (int)SoundType.MAXCOUNT; i++)
        {
            SoundSetting soundSetting = new SoundSetting(soundSettingAsset, (SoundType)i);
            soundSettings.Add(soundSetting);
            soundSettingContainer.Add(soundSetting._container);
        }

        foreach(var setting in soundSettings)
        {
        }
    }
}
