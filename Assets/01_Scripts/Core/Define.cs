using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectScene
{
    public struct InputKey
    {
        public KeyCode upKey;
        public KeyCode downKey;
        public KeyCode leftKey;
        public KeyCode rightKey;
        public KeyCode selectKey;
    }

    public enum Player
    {
        player1 = 1,
        player2 = 2,
        computer = 3,
    }

    public enum SelectState
    {
        none,
        focus,
        select,
    }

    public enum SceneEnum
    {
        Start,
        Loading,
        InGame,
        End,
    }
}

