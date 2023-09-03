using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Slot
{

    public bool IsFocused { get; set; }
    public bool IsSelected { get; set; }
    public VisualElement Element { get; set; }  
    public RenderTexture Image { get; set; }
    public int SlotIndex { get; set; }

    public Slot(VisualElement element, RenderTexture image, int idx)
    {
        Element = element;
        Image = image;
        SlotIndex = idx;
    }
}
