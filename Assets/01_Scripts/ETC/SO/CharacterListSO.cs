using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Character
{
    public string name;
    public string description;
    public Material material;
    //[HideInInspector, SerializeField] private ImageType imageType;
    //[HideInInspector, SerializeField] private RenderTexture renderTexture;
    //[HideInInspector, SerializeField] private Sprite sprite;
    //[HideInInspector, SerializeField] private Texture2D texture2d;
    public ImageType imageType;
    public RenderTexture renderTexture;
    public Sprite sprite;
    public Texture2D texture2d;
    public RenderTexture GetRenderTexture(){
        if(renderTexture != null)
            return renderTexture;
        else
            Debug.Log("Texture없음");
        return null;
    }

    public Sprite GetSprite()
    {
        if (sprite != null)
            return sprite;
        else
            Debug.Log("Texture없음");
        return null;
    }

    public Texture2D GetTexture2D()
    {
        if (texture2d != null)
            return texture2d;
        else
            Debug.Log("Texture없음");
        return null;
    }


}

[CreateAssetMenu(menuName = "SO/Character")]
public class CharacterListSO : ScriptableObject
{
    [SerializeField]
    public List<Character> List;

    public List<Character> GetCharacterList() => List;
}

