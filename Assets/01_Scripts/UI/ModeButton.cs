using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Core;
using System;

public class ModeButton
{
    Button _modBtn;
    Player _currentMod;
    Player _linkedPlayer;

    string player1 = "p1";
    string player2 = "p2";
    string computer = "cp";

    public ModeButton(Button button, Player player)
    {
        _linkedPlayer = player;
        _currentMod = player;
        _modBtn = button;
        _modBtn.RegisterCallback<ClickEvent>(SelectMode);
        //init
        PlayerSetting(_linkedPlayer);
    }

    public void SelectMode(ClickEvent evt)
    {
        switch (_currentMod) 
        {
            case Player.player1:
                CPSetting();
                break;
            case Player.player2:
                CPSetting();
                break;
            case Player.computer:
                PlayerSetting(_linkedPlayer);
                break;
        }
    }

    private void PlayerSetting(Player player)
    {
        if(player == Player.player1)
        {
            SetText("1p");
            RemoveAndAddClass(computer, player1);
        }
        else // p2
        {
            SetText("2p");
            RemoveAndAddClass(computer, player2);
        }
        _currentMod = player;
    }

    private void CPSetting()
    {
        _currentMod = Player.computer;
        SetText("cp");

        if (_linkedPlayer == Player.player1)
        {
            RemoveAndAddClass(player1, computer);
        }
        else
        {
            RemoveAndAddClass(player2, computer);
        }
    }

    public void RemoveAndAddClass(string removeclassname, string addclassname)
    {
        _modBtn.RemoveFromClassList(removeclassname);
        _modBtn.AddToClassList(addclassname);
    }

    public void SetText(string text)
    {
        _modBtn.text = text;
    }
}
