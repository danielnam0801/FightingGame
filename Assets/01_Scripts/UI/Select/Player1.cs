using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player1 : Selector
{
    public Player1(SelectUI selectUI, Player player, InputKey keys, List<Slot> slots) : base(selectUI, keys, slots)
    {
        this.player = player;
    }

    public override void Focus()
    {
        Slot currentslot = FindSlotByIndex(_curIdx);
        Slot prevslot = FindSlotByIndex(_prevIdx);

        currentslot.Element.AddToClassList("p1");
        prevslot.Element.RemoveFromClassList("p1");
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
        selectUI.SetLeftPanelImage(currentslot);
        Debug.Log("Player1 Setting");
        isSelect = true;
    }


}
