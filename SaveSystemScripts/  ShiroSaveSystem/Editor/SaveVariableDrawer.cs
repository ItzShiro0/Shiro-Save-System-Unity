#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SaveVariable))]
public class SaveVariableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty typeProp = property.FindPropertyRelative("type");
        SerializedProperty keyProp = property.FindPropertyRelative("key");

        if (typeProp == null || keyProp == null)
        {
            EditorGUI.HelpBox(position, "Error", MessageType.Error);
            EditorGUI.EndProperty();
            return;
        }

        float typeWidth = 70f;
        float keyWidth = (position.width - typeWidth) / 2f - 5f;
        float valueWidth = position.width - typeWidth - keyWidth - 10f;

        Rect typeRect = new Rect(position.x, position.y, typeWidth, position.height);
        EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);

        Rect keyRect = new Rect(position.x + typeWidth + 5f, position.y, keyWidth, position.height);
        EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none);

        Rect valueRect = new Rect(position.x + typeWidth + keyWidth + 10f, position.y, valueWidth, position.height);
        SaveDataType currentType = (SaveDataType)typeProp.enumValueIndex;

        SerializedProperty valueProp = null;
        switch (currentType)
        {
            case SaveDataType.Int: valueProp = property.FindPropertyRelative("intValue"); break;
            case SaveDataType.Float: valueProp = property.FindPropertyRelative("floatValue"); break;
            case SaveDataType.Bool: valueProp = property.FindPropertyRelative("boolValue"); break;
            case SaveDataType.String: valueProp = property.FindPropertyRelative("stringValue"); break;
        }

        if (valueProp != null)
        {
            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
        }

        EditorGUI.EndProperty();
    }
}
#endif