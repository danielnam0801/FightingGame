using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CharacterListSO))]
public class ChangePropertyTypeInspector : Editor
{
    SerializedProperty characterlist;
    CharacterListSO characterSO;
    private ReorderableList list = null;

    //private CharacterListSO _characterList;

    //private const string _helpText = "Cannot find 'CharacterListSO' component on any GameObject in the scene!";

    //private static Vector2 _windowsMinSize = Vector2.one * 500f;
    //private static Rect _helpRect = new Rect(0f, 0f, 400f, 100f);
    //private static Rect _listRect = new Rect(Vector2.zero, _windowsMinSize);


    private void OnEnable()
    {
        //characterSO = target as CharacterListSO;
        //characterlist = serializedObject.FindProperty("List");
        //list = new ReorderableList(serializedObject, characterlist, true, true, true, true);
        //list.drawElementCallback = DrawListItems;
        //list.drawHeaderCallback = DrawHeader;
   
    }

    private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        //var 
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.Foldout(rect, true, GUIContent.none);
       // EditorGUILayout.Foldout(true, GUIContent.none);
        EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Name");
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("name"), 
            GUIContent.none
        );
        EditorGUI.LabelField(new Rect(rect.x, rect.y + 100, 100, EditorGUIUtility.singleLineHeight), "Description");
        EditorGUI.PropertyField(new Rect(rect.x + 40, rect.y + 100, 100, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("description"), 
            GUIContent.none
        );
        EditorGUI.PropertyField(new Rect(rect.x, rect.y + 200, 100, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("imageType"), 
            GUIContent.none
        );
       // EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawHeader(Rect rect)
    {
        string name = "characters";
        EditorGUI.LabelField(rect, name, EditorStyles.boldLabel);
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();
        //base.OnInspectorGUI();
        //list.DoLayoutList();

        //serializedObject.ApplyModifiedProperties();

        //if (GUI.changed)
        //{
        //    EditorUtility.SetDirty(characterSO);
        //}
    }
}