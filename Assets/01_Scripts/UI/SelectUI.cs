using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UIElements;
using System;

public class SelectUI : MonoBehaviour
{
    [SerializeField] CharacterListSO _characterList;
    [SerializeField] VisualTreeAsset _slotAsset;

    UIDocument _uiDocument;

    VisualElement _LeftPanel;
    VisualElement _RightPanel;
    VisualElement _SlotPanel;
    Label _VSLabel;

    List<Slot> _characters = new List<Slot>();
    //List<MapSlot> _maps = new List<MapSlot>();

    Selector player1;
    Selector player2;

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

        _VSLabel = root.Q<Label>("VSLabel");
 
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
        //Slot 클릭이벤트 구현해야함  
    }

    private void Update()
    {
        player1?.Update();
        player2?.Update();
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

    public void SetLeftPanelImage(Slot slot)
    {
        _LeftPanel.style.backgroundImage = SetImage(slot.Image);
    }

    public void SetRightPanelImage(Slot slot)
    {
        _RightPanel.style.backgroundImage = SetImage(slot.Image);
    }
}
