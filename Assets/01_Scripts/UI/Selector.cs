using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Core;
using System.Linq;
using System;

public abstract class Selector
{
    public SelectState currentState;

    protected InputKey keys;
    protected Player player;
    protected int _curIdx;
    protected int _prevIdx;
    protected SelectUI selectUI;
    
    private List<Slot> slots;


    public Selector(SelectUI selectUI, InputKey keys, List<Slot> slots)
    {
        this.selectUI = selectUI;
        this.slots = slots;
        this.keys = keys;
    
        _curIdx = 0;
        _prevIdx = 0;
        currentState = SelectState.none;
    }

    public void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(keys.selectKey))
            {
                currentState = SelectState.select;
                Select();
            }
            else
            {
                currentState = SelectState.focus;
                Choose();
            }
        }
    }
    public void Choose()
    {
        if (Input.GetKeyDown(keys.leftKey))
        {
            MovingTarget(-1);
        }
        if (Input.GetKeyDown(keys.rightKey))
        {
            MovingTarget(1);
        }
    }

    private void MovingTarget(int idx)
    {
        _prevIdx = _curIdx;
        _curIdx += idx;

        if(_curIdx < 0)
            _curIdx = slots.Count() - 1;
        
        if(_curIdx >= slots.Count())
            _curIdx = 0;

        Focus();
    }
    protected Slot FindSlotByIndex(int idx)
    {
        return slots[idx];
    }

    public abstract void Focus();
    public abstract void Select();

}
