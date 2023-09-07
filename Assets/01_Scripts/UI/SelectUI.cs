using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UIElements;
using System;
using UnityEngine.Experimental.Rendering;

public class SelectUI : MonoBehaviour
{
    [SerializeField] CharacterListSO _characterList;
    [SerializeField] VisualTreeAsset _slotAsset;

    UIDocument _uiDocument;

    #region VisualElements

    VisualElement _LeftPanel;
    VisualElement _RightPanel;
    VisualElement _SlotPanel;
    VisualElement _ReadyPanel;
    
    Button _modBtnleft;
    Button _modBtnright;
    Button _returnBtn;

    Label _VSElement;
    Label _timeElement;

    #endregion

    List<Slot> _characters = new List<Slot>();
    //List<MapSlot> _maps = new List<MapSlot>();

    Selector player1;
    Selector player2;
    TimeLabel timeLabel;

    bool isSelectAll = false;
    
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;

        _LeftPanel = root.Q<VisualElement>("Middle--left");
        _RightPanel = root.Q<VisualElement>("Middle--right");
        _SlotPanel = root.Q<VisualElement>("SlotPanel");
        _ReadyPanel = root.Q<VisualElement>("ReadyPanel");
        _VSElement = root.Q<Label>("VSLabel");
        _timeElement = root.Q<Label>("TimeLabel");
        _returnBtn = root.Q<Button>("ReturnBtn");
        _modBtnleft = root.Q<Button>("1pBtn");
        _modBtnright = root.Q<Button>("2pBtn");


        MakeCharacterSlot();

        InputKey p1 = new InputKey();
        p1.upKey = KeyCode.W;
        p1.leftKey = KeyCode.A;
        p1.downKey = KeyCode.S;
        p1.rightKey = KeyCode.D;
        p1.selectKey = KeyCode.Space;
        
        InputKey p2 = new InputKey();
        p2.upKey = KeyCode.UpArrow;
        p2.leftKey = KeyCode.LeftArrow;
        p2.downKey = KeyCode.DownArrow;
        p2.rightKey = KeyCode.RightArrow;
        p2.selectKey = KeyCode.Return;

        player1 = new Player1(this, Player.player1, p1, _characters);
        player2 = new Player2(this, Player.player2, p2, _characters);
        //Slot Ŭ���̺�Ʈ �����ؾ���  

        timeLabel = new TimeLabel(this, _timeElement, 90f);
        ModeButton modeBtnleft = new ModeButton(_modBtnleft, Player.player1);
        ModeButton modeBtnright = new ModeButton(_modBtnright, Player.player2);

        _returnBtn.RegisterCallback<ClickEvent>((evt) =>
        {
            GoMain();
        });
    
    }

    private void Update()
    {
        if(player1.isSelect && player2.isSelect)
        {
            isSelectAll = true;
            _ReadyPanel.RemoveFromClassList("off");
        }
        else
        {
            isSelectAll = false;
            _ReadyPanel.AddToClassList("off");
        }

        if(isSelectAll)
        {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                GoToGameScene();
            }
        }

        player1?.Update();
        player2?.Update();
        timeLabel.Update();
    }

    private void MakeCharacterSlot()
    {
        for(int i = 0; i < _characterList.List.Count; i++)
        {
            VisualElement element = _slotAsset.Instantiate().Q("Slot");
            RenderTexture image = _characterList.List[i].texture;
           
            element.style.backgroundImage = SetImage(image);
            _SlotPanel.Add(element);

            CharacterSlot slot = new CharacterSlot(element, image, i);
            _characters.Add(slot);
        }
    }

    private StyleBackground SetImage(RenderTexture texture)
    {
        return new StyleBackground(Background.FromRenderTexture(texture));
    }

    private StyleBackground SetReverseImage(RenderTexture texture)
    {
        return new StyleBackground(Background.FromRenderTexture(texture));
    }

    public void SetLeftPanelImage(Slot slot)
    {
        _LeftPanel.style.backgroundImage = SetImage(slot.Image);
    }

    public void SetRightPanelImage(Slot slot)
    {
        _RightPanel.style.backgroundImage = SetReverseImage(slot.Image);
    }

    private void GoToGameScene()
    {
        //GameScene���� �̵�
        //���� ���õ� �������� ���� �Ѱ������
        // Player1,2 ĳ����, AI ����
        // ���� ��
        timeLabel.Stop = true;
        Debug.Log("GoGameScene");
    }

    public void GoMain()
    {
        //Main���� �̵�
        Debug.Log("GoMain");
    }
}
