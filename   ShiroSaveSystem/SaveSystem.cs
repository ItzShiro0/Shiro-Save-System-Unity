using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class SaveSystem
{
    private static Dictionary<string, RuntimeSaveData> _saves = new Dictionary<string, RuntimeSaveData>();
    private static SaveSchema[] _schemas;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        _schemas = Resources.LoadAll<SaveSchema>("");

        foreach (var schema in _schemas)
        {
            if (!string.IsNullOrWhiteSpace(schema.saveID))
            {
                LoadSchema(schema);
            }
        }
    }

    private static void LoadSchema(SaveSchema schema)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{schema.saveID}.json");
        RuntimeSaveData data;

        if (File.Exists(path) && SaveProtector.IsSaveFileValid(path))
        {
            data = JsonUtility.FromJson<RuntimeSaveData>(File.ReadAllText(path));
        }
        else
        {
            data = new RuntimeSaveData();
        }

        MergeDefaults(data, schema);
        _saves[schema.saveID] = data;
    }

    public static void SetInt(string saveID, string key, int value) => _saves[saveID].IntData[key] = value;
    public static int GetInt(string saveID, string key) => _saves[saveID].IntData.TryGetValue(key, out var val) ? val : 0;

    public static void SetFloat(string saveID, string key, float value) => _saves[saveID].FloatData[key] = value;
    public static float GetFloat(string saveID, string key) => _saves[saveID].FloatData.TryGetValue(key, out var val) ? val : 0f;

    public static void SetBool(string saveID, string key, bool value) => _saves[saveID].BoolData[key] = value;
    public static bool GetBool(string saveID, string key) => _saves[saveID].BoolData.TryGetValue(key, out var val) ? val : false;

    public static void SetString(string saveID, string key, string value) => _saves[saveID].StringData[key] = value;
    public static string GetString(string saveID, string key) => _saves[saveID].StringData.TryGetValue(key, out var val) ? val : string.Empty;

    public static void Save(string saveID)
    {
        if (!_saves.ContainsKey(saveID)) return;

        string path = Path.Combine(Application.persistentDataPath, $"{saveID}.json");
        string tempPath = path + ".tmp";
        string tempHashPath = tempPath + ".hash";

        try
        {
            string json = JsonUtility.ToJson(_saves[saveID], true);
            File.WriteAllText(tempPath, json);
            
            SaveProtector.GenerateHashForFile(tempPath);

            if (File.Exists(path)) File.Delete(path);
            if (File.Exists(path + ".hash")) File.Delete(path + ".hash");

            File.Move(tempPath, path);
            File.Move(tempHashPath, path + ".hash");

            Debug.Log($"Save Succed! Save path: {Application.persistentDataPath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Save Error {saveID}: {e.Message}");
        }
    }

    public static void SaveAll()
    {
        if (_schemas == null) return;
        foreach (var schema in _schemas)
        {
            if (!string.IsNullOrWhiteSpace(schema.saveID)) Save(schema.saveID);
        }
    }

    public static void Reset(string saveID)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{saveID}.json");
        if (File.Exists(path)) File.Delete(path);
        if (File.Exists(path + ".hash")) File.Delete(path + ".hash");

        foreach (var schema in _schemas)
        {
            if (schema.saveID == saveID)
            {
                LoadSchema(schema);
                Save(saveID);
                break;
            }
        }
    }

    private static void MergeDefaults(RuntimeSaveData data, SaveSchema schema)
    {
        foreach (var item in schema.variables)
        {
            switch (item.type)
            {
                case SaveDataType.Int:
                    if (!data.IntData.ContainsKey(item.key)) data.IntData[item.key] = item.intValue;
                    break;
                case SaveDataType.Float:
                    if (!data.FloatData.ContainsKey(item.key)) data.FloatData[item.key] = item.floatValue;
                    break;
                case SaveDataType.Bool:
                    if (!data.BoolData.ContainsKey(item.key)) data.BoolData[item.key] = item.boolValue;
                    break;
                case SaveDataType.String:
                    if (!data.StringData.ContainsKey(item.key)) data.StringData[item.key] = item.stringValue;
                    break;
            }
        }
    }
}