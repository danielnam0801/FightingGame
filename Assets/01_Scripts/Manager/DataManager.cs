using UnityEngine;

public class DataManager<T> : MonoBehaviour where T : class
{
    public static DataManager<T> Instance;

    public string ChangeDataToJson(T data)
    {
        return JsonUtility.ToJson(data);
    }

    public T LoadData(string data)
    {
        return JsonUtility.FromJson<T>(data);
    }
}
