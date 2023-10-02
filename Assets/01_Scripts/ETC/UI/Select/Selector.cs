using System.Collections.Generic;
using UnityEngine;
using Core;
using System.Linq;
using CustomUI;

namespace SelectScene
{

    public class Selector
    {
        public SelectState currentState;

        protected InputKey keys;
        protected PlayerType player;
        protected int _curIdx;
        public PlayerType GetPlayer => player;
        public int GetCurSlotIdx => _curIdx;

        protected int _prevIdx;
        protected int? _beforeSelectedIdx;
    
        private List<Slot> slots;

        public bool isSelect = false;

        public Selector(PlayerType player, InputKey keys, List<Slot> slots)
        {
            this.slots = slots;
            this.keys = keys;
            this.player = player;

            _curIdx = 0;
            _prevIdx = 0;
            _beforeSelectedIdx = null;
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

        public virtual void Focus()
        {
            Slot currentslot = FindSlotByIndex(_curIdx);
            Slot prevslot = FindSlotByIndex(_prevIdx);

            currentslot.Focused(player);
            prevslot.Arrived(player);

            if (_beforeSelectedIdx != null)
            {
                Slot beforeSelectedSlot = FindSlotByIndex(_beforeSelectedIdx.Value);
                beforeSelectedSlot.IsSelected -= 1;
                _beforeSelectedIdx = null;
            }
        
            isSelect = false;
        }

        public virtual void Select()
        {
            if (_beforeSelectedIdx == _curIdx) return;
            _beforeSelectedIdx = _curIdx;
       
            Slot currentslot = FindSlotByIndex(_curIdx);
            currentslot.Selected(player);
        
            isSelect = true;
        }
    
        protected Slot FindSlotByIndex(int idx)
        {
            return slots[idx];
        }

    }

}
