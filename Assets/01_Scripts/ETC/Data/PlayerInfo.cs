using System;


[Serializable]
class PlayerInfo : IJsonData
{
    public Character character;
    public PlayerType mode;
    public string Path { get; set; }
}