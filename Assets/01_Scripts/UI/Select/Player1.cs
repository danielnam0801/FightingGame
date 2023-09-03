using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player1 : Selector
{
  
    public Player1(SelectUI selectUI, Player player, InputKey keys, List<Slot> slots) : base(selectUI, player, keys, slots)
    {
    }

    public override void Focus()
    {
        Slot currentslot = FindSlotByIndex(_curIdx);
        Slot prevslot = FindSlotByIndex(_prevIdx);

        currentslot.Element.AddToClassList("p1");
        prevslot.Element.RemoveFromClassList("p1");
    }

    public override void Select()
    {
        Debug.Log("Player1 Setting");
        Slot currentslot = FindSlotByIndex(_curIdx);
        selectUI.SetLeftPanelImage(currentslot);
    }


}
