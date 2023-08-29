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
        //���� ������ ���� ��� ���

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
        //���߿� �÷��� �ƴ϶� 1p, 2p �ĳڷ� �ٲ������
        currentSlot.Element.style.backgroundColor = Color.red;
    }

    private Slot FindSlotByIndex(int idx)
    {
        return slots[idx];
    }

}
