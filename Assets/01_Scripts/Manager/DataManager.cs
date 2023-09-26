using System.IO;
using UnityEngine;

public static class DataManager<T> where T : IJsonData
{
    public static void SaveData(T data)
    {
        File.WriteAllText($"{Application.dataPath}/{data.Path}", JsonUtility.ToJson(data));
        Debug.Log("Save");
    }

    public static T LoadData(string path)
    {
        if(File.Exists($"{Application.dataPath}/{path}"))
        {
            Debug.Log("Load ����");
            return JsonUtility.FromJson<T>(File.ReadAllText($"{Application.dataPath}/{path}"));
        }
        else
        {
            Debug.Log("Load ����");
            return default(T);
        }
        
    }
}
