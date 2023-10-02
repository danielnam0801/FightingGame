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

    public enum SelectState
    {
        none,
        focus,
        select,
    }
}

public enum PlayerType
{
    player1 = 1,
    player2 = 2,
    AI = 3,
}

public enum SceneEnum
{
    Start,
    Loading,
    InGame,
    End,
}
public enum ImageType
{
    RenderTexture,
    Sprite,
    Texture2d,
}


