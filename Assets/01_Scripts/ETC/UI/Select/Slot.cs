using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SelectScene
{ 
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
        public StyleBackground Image { get; set; }
        public int SlotIndex { get; set; }

        private SelectUI selectUI;

        public Slot(SelectUI selectUI, VisualElement element, StyleBackground image, int idx)
        {
            Element = element;
            Image = image;
            SlotIndex = idx;
            this.selectUI = selectUI;   
        }

        public virtual void Arrived(PlayerType player)
        {
            switch(player)
            {
                case PlayerType.player1:
                    Element.RemoveFromClassList("p1");
                    break;
                case PlayerType.player2:
                    Element.RemoveFromClassList("p2");
                    break;
                default:
                    break;
            }
        }

        public virtual void Focused(PlayerType player)
        {
            switch (player)
            {
                case PlayerType.player1:
                    Element.AddToClassList("p1");
                    break;
                case PlayerType.player2:
                    Element.AddToClassList("p2");
                    break;
                default:
                    break;
            }
        }

        public virtual void Selected(PlayerType player)
        {
            IsSelected += 1;
            switch (player)
            {
                case PlayerType.player1:
                    selectUI.SetLeftPanelImage(this);
                    break;
                case PlayerType.player2:
                    selectUI.SetRightPanelImage(this);
                    break;
                default:
                    Debug.Log("플레이어가 아님");
                    break;
            }
        }
    }
}