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

/// <summary>
/// playerPostion when player spawned;
/// </summary>
public enum PlayerSpawnState
{
    left = 1, 
    right  = 2,
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

public enum Sound
{
    Bgm,
    Effect,
    MaxCount,  // 아무것도 아님. 그냥 Sound enum의 개수 세기 위해 추가. (0, 1, '2' 이렇게 2개) 
}

