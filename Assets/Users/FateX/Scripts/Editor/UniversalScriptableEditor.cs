using UnityEditor;
using UnityEngine;

public class UniversalScriptableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawProperties(serializedObject.GetIterator());

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawProperties(SerializedProperty prop)
    {
        SerializedProperty iterator = prop.Copy();
        bool enterChildren = true;

        while (iterator.NextVisible(enterChildren))
        {
            if (iterator.propertyPath == "m_Script")
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(iterator);
                EditorGUI.EndDisabledGroup();
                enterChildren = false;
                continue;
            }

            DrawPropertyRecursive(iterator);
            enterChildren = false;
        }
    }

    private void DrawPropertyRecursive(SerializedProperty prop)
    {
        // Массивы (кроме строки)
        if (prop.isArray && prop.propertyType != SerializedPropertyType.String)
        {
            prop.isExpanded = EditorGUILayout.Foldout(prop.isExpanded, prop.displayName, true);
            if (prop.isExpanded)
            {
                EditorGUI.indentLevel++;

                for (int i = 0; i < prop.arraySize; i++)
                {
                    EditorGUILayout.BeginVertical("box");
                    var element = prop.GetArrayElementAtIndex(i);
                    DrawPropertyRecursive(element);

                    if (GUILayout.Button("Удалить"))
                        prop.DeleteArrayElementAtIndex(i);

                    EditorGUILayout.EndVertical();
                }

                if (GUILayout.Button("Добавить"))
                    prop.InsertArrayElementAtIndex(prop.arraySize);

                EditorGUI.indentLevel--;
            }
        }
        else if (prop.hasVisibleChildren && prop.propertyType != SerializedPropertyType.String)
        {
            // Вложенные объекты
            prop.isExpanded = EditorGUILayout.Foldout(prop.isExpanded, prop.displayName, true);
            if (prop.isExpanded)
            {
                EditorGUI.indentLevel++;
                var iterator = prop.Copy();
                var end = iterator.GetEndProperty();
                iterator.NextVisible(true);
                while (!SerializedProperty.EqualContents(iterator, end))
                {
                    DrawPropertyRecursive(iterator);
                    if (!iterator.NextVisible(false)) break;
                }
                EditorGUI.indentLevel--;
            }
        }
        else
        {
            DrawWithColorIfZero(prop);
        }
    }

    private void DrawWithColorIfZero(SerializedProperty prop)
    {
        bool isZero =
            (prop.propertyType == SerializedPropertyType.Float && Mathf.Approximately(prop.floatValue, 0f)) ||
            (prop.propertyType == SerializedPropertyType.Integer && prop.intValue == 0);

        var oldColor = GUI.color;
        if (isZero) GUI.color = new Color(1f, 0.6f, 0.6f);

        EditorGUILayout.PropertyField(prop, true);

        GUI.color = oldColor;
    }
}
