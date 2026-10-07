#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SaveSystemSetup
{
    [MenuItem("Tools/Save System/Create New Schema")]
    public static void CreateSchema()
    {
        string folderPath = "Assets/Resources";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/NewSaveSchema.asset");

        SaveSchema newSchema = ScriptableObject.CreateInstance<SaveSchema>();

        AssetDatabase.CreateAsset(newSchema, assetPath);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newSchema;
    }
}

[CustomEditor(typeof(SaveSchema))]
public class SaveSchemaEditor : Editor
{
    SerializedProperty saveIDProp;
    SerializedProperty variablesProp;
    private bool _isEditingName;

    void OnEnable()
    {
        saveIDProp = serializedObject.FindProperty("saveID");
        variablesProp = serializedObject.FindProperty("variables");
        
        if (saveIDProp == null) return;

        if (string.IsNullOrWhiteSpace(saveIDProp.stringValue) || saveIDProp.stringValue == "default_save")
        {
            _isEditingName = true;
        }
    }

    public override void OnInspectorGUI()
    {
        if (saveIDProp == null || variablesProp == null) return;

        serializedObject.Update();

        if (_isEditingName)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Enter a unique save file name to proceed.", MessageType.Info);
            
            EditorGUILayout.PropertyField(saveIDProp, new GUIContent("Save file name"));

            EditorGUILayout.Space();
            if (GUILayout.Button("Save Name", GUILayout.Height(30)))
            {
                if (!string.IsNullOrWhiteSpace(saveIDProp.stringValue))
                {
                    _isEditingName = false;
                    GUI.FocusControl(null);

                    string newName = saveIDProp.stringValue;
                    string assetPath = AssetDatabase.GetAssetPath(target);

                    serializedObject.ApplyModifiedProperties();
                    
                    AssetDatabase.RenameAsset(assetPath, newName);
                    AssetDatabase.SaveAssets();
                    return; 
                }
                else
                {
                    Debug.LogWarning("Save name cannot be empty!");
                }
            }
        }
        else
        {
            EditorGUILayout.Space();
            
            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(saveIDProp, new GUIContent("Save file name"));
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Edit", GUILayout.Width(50)))
            {
                _isEditingName = true;
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            
            EditorGUILayout.PropertyField(variablesProp, new GUIContent("Variable storage"), true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

public class SaveSchemaNameProtector : UnityEditor.AssetModificationProcessor
{
    private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
    {
        if (sourcePath.EndsWith(".asset"))
        {
            SaveSchema schema = AssetDatabase.LoadMainAssetAtPath(sourcePath) as SaveSchema;
            if (schema != null)
            {
                string newName = System.IO.Path.GetFileNameWithoutExtension(destinationPath);

                if (newName.Contains("NewSaveSchema")) return AssetMoveResult.DidNotMove;

                if (!string.IsNullOrEmpty(schema.saveID) && newName != schema.saveID)
                {
                    return AssetMoveResult.FailedMove;
                }
            }
        }
        return AssetMoveResult.DidNotMove;
    }
}
#endif