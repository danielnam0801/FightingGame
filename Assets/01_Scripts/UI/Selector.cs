using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Core;
using System.Linq;
using System;

public enum SelectState
{
    None,
    Choosing,
    Selected,
}

public class Selector
{
    public SelectState currentState;
    protected VisualElement _target;

    InputKey _keys;

    List<Slot> slots;
    int _curIdx;

    //protected Selector(VisualElement targetpanel, InputKey keys, List<Slot> slots)
    //{
    //    currentState = SelectState.None;
    //    _target = targetpanel;
    //    _keys = keys;
    //    _curIdx = 0;
    //    this.slots = slots;
    //}

    public Selector(InputKey keys, List<Slot> slots)
    {
        currentState = SelectState.None;
        _keys = keys;
        _curIdx = 0;
        this.slots = slots;
    }

    public void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(_keys.selectKey))
            {
                currentState = SelectState.Selected;
                Select();
            }
            else
            {
                currentState = SelectState.Choosing;
                Choose();
            }
        }
    }

    public virtual void Select()
    {
        //선택 됐을때 사운드 재생 등등

    }

    public void Choose()
    {
        if (Input.GetKeyDown(_keys.leftKey))
        {
            MovingTarget(-1);
        }
        if (Input.GetKeyDown(_keys.rightKey))
        {
            MovingTarget(1);
        }
    }

    private void MovingTarget(int idx)
    {
        _curIdx += idx;
        if(_curIdx < 0)
            _curIdx = slots.Count() - 1;
        
        if(_curIdx >= slots.Count())
            _curIdx = 0;


        Slot currentSlot = FindSlotByIndex(idx);
        //나중에 컬러가 아니라 1p, 2p 파넬로 바꿔줘야함
        currentSlot.Element.style.backgroundColor = Color.red;
    }

    private Slot FindSlotByIndex(int idx)
    {
        return slots[idx];
    }

}
