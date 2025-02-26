using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelEntry))]
public class LevelEntryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Ottieni il riferimento all'oggetto
        LevelEntry levelEntry = (LevelEntry)target;

        // Modifica la struttura
        SerializedProperty dataProp = serializedObject.FindProperty("data");

        // Visualizza la proprietà base
        serializedObject.Update();
        EditorGUILayout.PropertyField(dataProp, true);
        serializedObject.ApplyModifiedProperties();
    }
}
