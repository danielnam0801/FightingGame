using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUI;
using UnityEngine.UIElements;

public class Selector : MonoBehaviour
{
    [SerializeField] CharacterListSO _characterList;
    [SerializeField] VisualTreeAsset characterPanel;

    UIDocument _uiDocument;

    VisualElement _LeftPanel;
    VisualElement _RightPanel;
    VisualElement _SelectPanel;
    Label _VSLabel;
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;

        _LeftPanel = root.Q<VisualElement>("Middle--left");
        _RightPanel = root.Q<VisualElement>("Middle--right");
        _SelectPanel = root.Q<VisualElement>("CharacterSet");

        _VSLabel = root.Q<Label>("VSLabel");
 
        MakeCharPanel();
    }

    private void MakeCharPanel()
    {
        for(int i = 0; i < _characterList.List.Count; i++)
        {
            VisualElement template = characterPanel.Instantiate().Q("CharacterVisual");
            _SelectPanel.Add(template);
        }
    }
    
    public void Selected(int selectid, CharacterSlot slot)
    {
        Background bg = new Background();
        bg.renderTexture = slot.image;

        if (selectid == 1) // 1p
            _LeftPanel.style.backgroundImage = bg;
        else // 2p
            _RightPanel.style.backgroundImage = bg;

    }
}
