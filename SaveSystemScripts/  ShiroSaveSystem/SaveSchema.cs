using System;
using System.Collections.Generic;
using UnityEngine;

public enum SaveDataType { Int, Float, Bool, String }

[Serializable]
public struct SaveVariable
{
    public SaveDataType type;
    public string key;
    public int intValue;
    public float floatValue;
    public bool boolValue;
    public string stringValue;
}

[CreateAssetMenu(fileName = "NewSaveSchema", menuName = "Save System/Save Schema")]
public class SaveSchema : ScriptableObject
{
    public string saveID = "";
    public List<SaveVariable> variables = new List<SaveVariable>();
}