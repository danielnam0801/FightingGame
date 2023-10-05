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
   
    public SoundSetting(VisualTreeAsset asset, SoundType type)
    {
        Type = type;
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

        #endregion
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

    private void Record()
    {

    }
}
