using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RuntimeSaveData : ISerializationCallbackReceiver
{
    [SerializeField] List<string> _intKeys = new List<string>();
    [SerializeField] List<AntiCheatInt> _intValues = new List<AntiCheatInt>();

    [SerializeField] List<string> _floatKeys = new List<string>();
    [SerializeField] List<AntiCheatFloat> _floatValues = new List<AntiCheatFloat>();

    [SerializeField] List<string> _boolKeys = new List<string>();
    [SerializeField] List<AntiCheatBool> _boolValues = new List<AntiCheatBool>();

    [SerializeField] List<string> _stringKeys = new List<string>();
    [SerializeField] List<AntiCheatString> _stringValues = new List<AntiCheatString>();

    public Dictionary<string, AntiCheatInt> IntData = new Dictionary<string, AntiCheatInt>();
    public Dictionary<string, AntiCheatFloat> FloatData = new Dictionary<string, AntiCheatFloat>();
    public Dictionary<string, AntiCheatBool> BoolData = new Dictionary<string, AntiCheatBool>();
    public Dictionary<string, AntiCheatString> StringData = new Dictionary<string, AntiCheatString>();

    public void OnBeforeSerialize()
    {
        _intKeys.Clear();
        _intValues.Clear();

        _floatKeys.Clear();
        _floatValues.Clear();

        _boolKeys.Clear();
        _boolValues.Clear();

        _stringKeys.Clear();
        _stringValues.Clear();

        foreach (var kvp in IntData)
        {
            _intKeys.Add(kvp.Key);
            _intValues.Add(kvp.Value);
        }

        foreach (var kvp in FloatData)
        {
            _floatKeys.Add(kvp.Key);
            _floatValues.Add(kvp.Value);
        }

        foreach (var kvp in BoolData)
        {
            _boolKeys.Add(kvp.Key);
            _boolValues.Add(kvp.Value);
        }

        foreach (var kvp in StringData)
        {
            _stringKeys.Add(kvp.Key);
            _stringValues.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        IntData.Clear();
        FloatData.Clear();
        BoolData.Clear();
        StringData.Clear();

        for (int i = 0; i < _intKeys.Count; i++)
        {
            IntData[_intKeys[i]] = _intValues[i];
        }

        for (int i = 0; i < _floatKeys.Count; i++)
        {
            FloatData[_floatKeys[i]] = _floatValues[i];
        }

        for (int i = 0; i < _boolKeys.Count; i++)
        {
            BoolData[_boolKeys[i]] = _boolValues[i];
        }

        for (int i = 0; i < _boolKeys.Count; i++)
        {
            StringData[_stringKeys[i]] = _stringValues[i];
        }
    }
}