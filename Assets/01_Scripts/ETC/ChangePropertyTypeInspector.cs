using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CharacterListSO))]
public class ChangePropertyTypeInspector : Editor
{
    private SerializedProperty _characterList;

    private void OnEnable()
    {
        _characterList = serializedObject.FindProperty("List");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        for(int i = 0; i < _characterList.arraySize; i++)
        {
            var obj = _characterList.GetArrayElementAtIndex(i);
            switch ((ImageType)obj.FindPropertyRelative("imageType").intValue)
            {
                case ImageType.Sprite:
                    Debug.Log("Sprite");
                    EditorGUILayout.PropertyField(obj.FindPropertyRelative("sprite"));
                    break;
                case ImageType.RenderTexture:
                    EditorGUILayout.PropertyField(obj.FindPropertyRelative("renderTexture"));
                    Debug.Log("RenderTExture");
                    break;
                case ImageType.Texture2d:
                    EditorGUILayout.PropertyField(obj.FindPropertyRelative("texture2d"));
                    Debug.Log("Texture2d");
                    break;
            }
        }
        //Debug.Log(_characterList);
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("_objectType"));
        serializedObject.ApplyModifiedProperties();
    }
    //public void SetMatchProperty<T0, T1>() where T0: enum where T1: class
    //{

    //}
}
