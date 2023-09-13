using System;
using System.Collections.Generic;
using UnityEngine;


public enum ImageType
{
    RenderTexture,
    Sprite,
    Texture2d,
}

[Serializable]
public class Character
{
    public int characterNum;
    public string name;
    public string description;

    [SerializeField] private ImageType imageType;
    [HideInInspector][SerializeField] private RenderTexture renderTexture;
    [HideInInspector][SerializeField] private Sprite sprite;
    [HideInInspector][SerializeField] private Texture2D texture2d;

    public RenderTexture GetRenderTexture(){
        if(renderTexture != null)
            return renderTexture;
        else
            Debug.Log("Texture����");
        return null;
    }

    public Sprite GetSprite()
    {
        if (sprite != null)
            return sprite;
        else
            Debug.Log("Texture����");
        return null;
    }

    public Texture2D GetTexture2D()
    {
        if (texture2d != null)
            return texture2d;
        else
            Debug.Log("Texture����");
        return null;
    }


}

[CreateAssetMenu(menuName = "SO/Character")]
public class CharacterListSO : ScriptableObject
{
    [SerializeField]
    private List<Character> List;

    public List<Character> GetCharacterList() => List;
}

