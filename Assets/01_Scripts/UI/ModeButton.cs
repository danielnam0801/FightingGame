using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Core;

public class ModeButton
{
    Button _modBtn;
    Player _currentMod;

    string player1 = "p1";
    string player2 = "p2";
    string computer = "cp";

    public ModeButton(Button button, Player player)
    {
        _currentMod = player;
        _modBtn = button;
        _modBtn.RegisterCallback<ClickEvent>(SelectMode);
    }

    public void SelectMode(ClickEvent evt)
    {
        switch (_currentMod) 
        {
            case Player.player1:
                _currentMod = Player.player2;
                RemoveAndAddClass(player1, player2);
                break;
            case Player.player2:
                _currentMod = Player.computer;
                RemoveAndAddClass(player2, computer);
                break;
            case Player.computer:
                _currentMod = Player.player1;
                RemoveAndAddClass(computer, player1);
                break;
        }
    }

    public void RemoveAndAddClass(string removeclassname, string addclassname)
    {
        _modBtn.RemoveFromClassList(removeclassname);
        _modBtn.AddFromClassList(addclassname);
    }
}
