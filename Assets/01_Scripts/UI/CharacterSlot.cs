using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterSlot
{
    public bool IsStaying { get; set; }
    public bool IsSelected { get; set; }
    public int idx;
    public RenderTexture image;

    VisualElement slot;
    Selector selector;

    public CharacterSlot(VisualElement slot, Character , Selector selector)
    {
        this.idx = idx;
        this.image = image;
        this.selector = selector;
        IsSelected = false;
        IsStaying = false;   
    }


}
