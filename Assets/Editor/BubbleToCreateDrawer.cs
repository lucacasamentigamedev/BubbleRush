using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BubbleToCreate))]
public class BubbleToCreateDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Indentazione
        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            SerializedProperty type = property.FindPropertyRelative("type");
            SerializedProperty setFiller = property.FindPropertyRelative("setFiller");
            SerializedProperty minSpawn = property.FindPropertyRelative("min_Spawn");
            SerializedProperty maxSpawn = property.FindPropertyRelative("max_Spawn");
            SerializedProperty minPop = property.FindPropertyRelative("min_Pop");
            SerializedProperty maxPop = property.FindPropertyRelative("max_Pop");

            // Mostra le proprietà base
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), type);

            position.y += EditorGUIUtility.singleLineHeight;

            // Blocchiamo il check su altri elementi se uno è attivo
            EditorGUI.BeginChangeCheck();
            bool newSetFiller = EditorGUI.Toggle(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), "Set Filler", setFiller.boolValue);
            if (EditorGUI.EndChangeCheck() && newSetFiller)
            {
                DisableOtherSetFillers(property);
                setFiller.boolValue = true;
            }

            // Se setFiller è FALSE, mostra min/maxSpawn
            if (!setFiller.boolValue)
            {
                position.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), minSpawn);

                position.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), maxSpawn);
            }

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), minPop);

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), maxPop);

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;

        if (property.isExpanded)
        {
            height += EditorGUIUtility.singleLineHeight * 2; // type, setFiller
            if (!property.FindPropertyRelative("setFiller").boolValue)
                height += EditorGUIUtility.singleLineHeight * 2; // min/max Spawn
            height += EditorGUIUtility.singleLineHeight * 2; // min/max Pop
        }

        return height;
    }

    private void DisableOtherSetFillers(SerializedProperty property)
    {
        SerializedProperty bubblesArray = property.serializedObject.FindProperty("data").FindPropertyRelative("bubbles");

        for (int i = 0; i < bubblesArray.arraySize; i++)
        {
            SerializedProperty element = bubblesArray.GetArrayElementAtIndex(i);
            SerializedProperty setFiller = element.FindPropertyRelative("setFiller");

            if (element.propertyPath != property.propertyPath)
            {
                setFiller.boolValue = false;
            }
        }

        property.serializedObject.ApplyModifiedProperties();
    }
}
