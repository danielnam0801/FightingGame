
[Serializable]
public class CharacterInfo
{
    public int characterNum;
    public string name;
    public string description;
    public RenderTexture _image;
}

[CreateAssetMenu(menuName = "SO/Character")]
public class CharacterListSO : ScriptableObject
{
    public List<CharacterInfo> List;
}

