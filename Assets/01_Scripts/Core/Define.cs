using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public struct InputKey
    {
        public KeyCode upKey;
        public KeyCode downKey;
        public KeyCode leftKey;
        public KeyCode rightKey;
        public KeyCode selectKey;
    }

    public enum SceneEnum
    {
        Start,
        Loading,
        InGame,
        End,
    }
}

