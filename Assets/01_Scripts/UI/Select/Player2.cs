using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player2 : Selector
{
    public Player2(SelectUI selectUI, Player player, InputKey keys, List<Slot> slots) : base(selectUI, player, keys, slots)
    {
    }

    public override void Focus()
    {
        Slot currentslot = FindSlotByIndex(_curIdx);
        Slot prevslot = FindSlotByIndex(_prevIdx);

        currentslot.Element.AddToClassList("p2");
        prevslot.Element.RemoveFromClassList("p2");
    }

    public override void Select()
    {
        Debug.Log("Player2 Setting");
        Slot currentslot = FindSlotByIndex(_curIdx);
        selectUI.SetRightPanelImage(currentslot);
    }
}
