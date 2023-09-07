using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player2 : Selector
{
    public Player2(SelectUI selectUI, Player player, InputKey keys, List<Slot> slots) : base(selectUI, keys, slots)
    {
        this.player = player;
    }

    public override void Focus()
    {
        Slot currentslot = FindSlotByIndex(_curIdx);
        Slot prevslot = FindSlotByIndex(_prevIdx);

        currentslot.Element.AddToClassList("p2");
        prevslot.Element.RemoveFromClassList("p2");

        if (_beforeSelectedIdx != null)
        {
            Slot beforeSelectedSlot = FindSlotByIndex(_beforeSelectedIdx.Value);
            beforeSelectedSlot.IsSelected -= 1;
            _beforeSelectedIdx = null;
        }
        isSelect = false;
    }

    public override void Select()
    {
        if (_beforeSelectedIdx == _curIdx) return;
        _beforeSelectedIdx = _curIdx;   
        Slot currentslot = FindSlotByIndex(_curIdx);
        currentslot.IsSelected += 1;
        selectUI.SetRightPanelImage(currentslot);
        Debug.Log("Player2 Setting");
        isSelect = true;
    }
}
