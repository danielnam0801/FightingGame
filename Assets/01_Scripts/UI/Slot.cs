using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Slot
{

    public bool IsFocused { get; set; }
    public bool IsSelected { get; set; }
    public VisualElement Element { get; set; }  
    public int SlotIndex { get; set; }

    public Slot(VisualElement element, int idx)
    {
        Element = element;
        SlotIndex = idx;
    }
}
