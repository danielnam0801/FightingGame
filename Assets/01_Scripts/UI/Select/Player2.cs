using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player2 : Selector
{
    VisualElement targetPanel;
    public Player2(Player player, InputKey keys, List<Slot> slots, VisualElement target) : base(player, keys, slots)
    {
        this.targetPanel = target;
    }

    public override void Focus()
    {
        Slot currentslot = FindSlotByIndex(_curIdx);
        Slot prevslot = FindSlotByIndex(_prevIdx);

        currentslot.Element.AddToClassList("focus");
        currentslot.Element.AddToClassList("p2");
        prevslot.Element.RemoveFromClassList("focus");
        prevslot.Element.RemoveFromClassList("p2");
    }

    public override void Select()
    {
        Slot currentslot = FindSlotByIndex(_curIdx);

        currentslot.Element.RemoveFromClassList("focus");
        currentslot.Element.AddToClassList("select");

        targetPanel.style.backgroundImage = currentslot.Element.style.backgroundImage;
    }
}
