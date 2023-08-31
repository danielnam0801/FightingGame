using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Core;
using System.Linq;
using System;

public enum Player 
{ 
    player1,
    player2,
}

public enum SelectState
{
    none,
    focus,
    select,
}

public abstract class Selector
{
    public SelectState currentState;

    protected Sprite target;
    protected InputKey keys;
    protected Player player;
    protected int _curIdx;
    protected int _prevIdx;

    List<Slot> slots;

    public Selector(Player player, InputKey keys, List<Slot> slots)
    {
        this.slots = slots;
        this.player = player;
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
                Debug.Log("selelct");
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
