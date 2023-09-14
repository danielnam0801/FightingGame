using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CharacterListSO))]
public class ChangePropertyTypeInspector : EditorWindow
{
    private bool _isActive;
    private SerializedObject _characterSO = null;
    private ReorderableList _listRE = null;

    private CharacterListSO _characterList;
 
    private const string _helpText = "Cannot find 'CharacterListSO' component on any GameObject in the scene!";
 
    private static Vector2 _windowsMinSize = Vector2.one * 500f;
    private static Rect _helpRect = new Rect(0f, 0f, 400f, 100f);
    private static Rect _listRect = new Rect(Vector2.zero, _windowsMinSize);
    
    private void OnEnable()
    {
        //_characterList = Resources.Load<CharacterListSO>("CharacterList");
        _characterList = FindObjectOfType<CharacterListSO>();
        if (_characterList)
        {
            _characterSO = new SerializedObject(_characterList);
            //Init
            _listRE = new ReorderableList(_characterSO, _characterSO.FindProperty("List")
                , true, true, true, true);
            Debug.Log(_listRE);
            _listRE.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Characters");
            _listRE.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += 2f;
                rect.height = EditorGUIUtility.singleLineHeight;
                GUIContent objectLabel = new GUIContent($"Character {index}");
                EditorGUI.PropertyField(rect, _listRE.serializedProperty.GetArrayElementAtIndex(index), objectLabel);
            };

        }
    }
    
    private void OnInspectorUpdate()
    {
        Repaint();
    }

    public void OnGUI()
    {
        //base.OnInspectorGUI();
        OnInspectorUpdate();
    
        if (_characterSO == null)
        {
            EditorGUI.HelpBox(_helpRect, _helpText, MessageType.Warning);
            return;
        }
        else if (_characterSO != null)
        {
            _characterSO.Update();
            _listRE.DoList(_listRect);
            _characterSO.ApplyModifiedProperties();
        }
        
        GUILayout.Space(_listRE.GetHeight() + 30f);
        GUILayout.Label("Please select Game Objects to simulate");
        GUILayout.Space(10f);
 
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(30f);
    }

    private void DrawCharacterList(SerializedProperty prop)
    {
        EditorGUILayout.PropertyField(prop);
        EditorGUI.indentLevel += 1;
        for (int i = 0; i < prop.arraySize; i++)
        {
            EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i));    
        }

        EditorGUI.indentLevel -= 1;
        // EditorGUILayout.PropertyField(prop.FindPropertyRelative("characterNum"));
        // EditorGUILayout.PropertyField(prop.FindPropertyRelative("name"));
        // EditorGUILayout.PropertyField(prop.FindPropertyRelative("description"));
        // EditorGUILayout.PropertyField(prop.FindPropertyRelative("imageType"));

        // switch ((ImageType)prop.FindPropertyRelative("imageType").intValue)
        // {
        //     case ImageType.Sprite:
        //         EditorGUILayout.PropertyField(prop.FindPropertyRelative("sprite"));
        //         break;
        //     case ImageType.RenderTexture:
        //         EditorGUILayout.PropertyField(prop.FindPropertyRelative("renderTexture"));
        //         break;
        //     case ImageType.Texture2d:
        //         EditorGUILayout.PropertyField(prop.FindPropertyRelative("texture2d"));
        //         break;
        // }
        // serializedObject.ApplyModifiedProperties();
    }
}
