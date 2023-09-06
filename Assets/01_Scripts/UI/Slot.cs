using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Slot
{

    public bool IsFocused { get; set; }

    private int isSelected;
    public int IsSelected
    {
        get { return isSelected; }
        set 
        {
            isSelected = value;
            isSelected = Mathf.Clamp(isSelected, 0, 2);
           
            if (isSelected == 0) 
            {
                Element.RemoveFromClassList("select");
            }
            else // Select가 1개 이상인 경우
            {
                if(!Element.ClassListContains("select"))
                    Element.AddToClassList("select");
            }
        }
    }

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
