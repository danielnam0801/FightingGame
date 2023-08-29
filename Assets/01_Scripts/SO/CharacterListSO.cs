using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character
{
    public int characterNum;
    public string name;
    public string description;
    public RenderTexture texure;
}

[CreateAssetMenu(menuName = "SO/Character")]
public class CharacterListSO : ScriptableObject
{
    public List<Character> List;
}

