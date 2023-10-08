using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundSetting
{
    public SoundType Type;

    private Label _soundNameLabel;
    private Label _recordStateLabel;
    private Button _recordStartBtn;
    private Button _recordStoptBtn;
    private Button _listenBtn;
    private VisualElement _stateMark;
    public VisualElement _container;
    
    private SoundRecoding _recoding;
    private SettingUI _setting;

    public int Index;
    private bool canClick = true;

    public SoundSetting(SettingUI setting, VisualTreeAsset asset, SoundType type, int index)
    {
        Index = index;
        Type = type;
        _setting = setting;

        #region VisualElement
        _container = asset.Instantiate().Q<VisualElement>("SoundSetting");
        _soundNameLabel = _container.Q<Label>("SoundName");
        _recordStateLabel = _container.Q<Label>("StateLabel");
        _recordStartBtn = _container.Q<Button>("RecordStart");
        _recordStoptBtn = _container.Q<Button>("RecordStop");
        _listenBtn = _container.Q<Button>("Listen");
        _stateMark = _container.Q<VisualElement>("StateMark");
        #endregion

        Init();
        #region CallbackEvt
        _recordStartBtn.RegisterCallback<ClickEvent>(RecordStart);
        _recordStoptBtn.RegisterCallback<ClickEvent>(RecordStop);
        _listenBtn.RegisterCallback<ClickEvent>(Listen);
        #endregion
    }


    private void RecordStart(ClickEvent evt)
    {
        if (!canClick) return;
        _setting.SetRecordingTrue(Index);
        _recoding.StartRecording();
    }

    private void RecordStop(ClickEvent evt)
    {
        if (!canClick) return;
        _recoding.StopRecording();
        _setting.SetRecordingFalse(Index);
    }

    public void SetClickLock(bool value)
    {
        if (value == true)
        {
            _recordStartBtn.AddToClassList("lock");
            _recordStoptBtn.AddToClassList("lock");
            _listenBtn.AddToClassList("lock");
            canClick = false;
        }
        else
        {
            _recordStartBtn.RemoveFromClassList("lock");
            _recordStoptBtn.RemoveFromClassList("lock");
            _listenBtn.RemoveFromClassList("lock");
            canClick = true;
        }
    }

    private void Listen(ClickEvent evt)
    {
        if (!canClick) return;
        SoundManager.Instance.PlaySound(Type);
    }

    private void Init()
    {
        _recoding = new SoundRecoding(Type);
        _soundNameLabel.text = NameText(Type);
    }

    private string NameText(SoundType soundType)
    {
        string name = string.Empty;
        switch(soundType)
        {
            case SoundType.Start:
                name = "������ �� �Ҹ�";
                break;
            case SoundType.Hit:
                name = "���� �� �Ҹ�";
                break;
            case SoundType.Block:
                name = "���� �� �Ҹ�";
                break;
            case SoundType.Attack:
                name = "���� �Ҹ�";
                break;
            case SoundType.BGM:
                name = "���";
                break;
            case SoundType.Win:
                name = "�¸� �Ҹ�";
                break;
            default:
                name = "�Ҹ� ����";
                break;
        }
        return name;
    }

}
