using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StateManager))]
public class StateManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty state1 = serializedObject.FindProperty("_state1");
        SerializedProperty state2 = serializedObject.FindProperty("_state2");

        EditorGUILayout.ObjectField(state1);
        EditorGUILayout.ObjectField(state2);

        string error = "";
        if (!(state1.objectReferenceValue is IStateable) || !(state2.objectReferenceValue is IStateable))
            error = "State must implement class 'IStateable'";
        if (state1 == null || state2 == null)
            error = "No state can be null";
        if (error != "")
            EditorGUILayout.HelpBox(error, MessageType.Error);

        serializedObject.ApplyModifiedProperties();
    }
}
