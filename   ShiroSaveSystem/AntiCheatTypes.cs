using System;
using System.Text;
using UnityEngine;

[Serializable]
public struct AntiCheatInt
{
    [SerializeField] private int cryptoKey;
    [SerializeField] private int hiddenValue;

    public AntiCheatInt(int value)
    {
        cryptoKey = UnityEngine.Random.Range(1000, 99999);
        hiddenValue = value ^ cryptoKey;
    }

    public int Value
    {
        get => hiddenValue ^ cryptoKey;
        set
        {
            cryptoKey = UnityEngine.Random.Range(1000, 99999);
            hiddenValue = value ^ cryptoKey;
        }
    }

    public static implicit operator int(AntiCheatInt encrypted) => encrypted.Value;
    public static implicit operator AntiCheatInt(int normal) => new AntiCheatInt(normal);
    public override string ToString() => Value.ToString();
}

[Serializable]
public struct AntiCheatFloat
{
    [SerializeField] private int cryptoKey;
    [SerializeField] private int hiddenValue;

    public AntiCheatFloat(float value)
    {
        cryptoKey = UnityEngine.Random.Range(1000, 99999);
        int intBits = BitConverter.SingleToInt32Bits(value);
        hiddenValue = intBits ^ cryptoKey;
    }

    public float Value
    {
        get
        {
            int intBits = hiddenValue ^ cryptoKey;
            return BitConverter.Int32BitsToSingle(intBits);
        }
        set
        {
            cryptoKey = UnityEngine.Random.Range(1000, 99999);
            int intBits = BitConverter.SingleToInt32Bits(value);
            hiddenValue = intBits ^ cryptoKey;
        }
    }

    public static implicit operator float(AntiCheatFloat encrypted) => encrypted.Value;
    public static implicit operator AntiCheatFloat(float normal) => new AntiCheatFloat(normal);
    public override string ToString() => Value.ToString();
}

[Serializable]
public struct AntiCheatBool
{
    [SerializeField] private int cryptoKey;
    [SerializeField] private int hiddenValue;

    public AntiCheatBool(bool value)
    {
        cryptoKey = UnityEngine.Random.Range(1000, 99999);
        int intValue = value ? 1 : 0;
        hiddenValue = intValue ^ cryptoKey;
    }

    public bool Value
    {
        get
        {
            int intValue = hiddenValue ^ cryptoKey;
            return intValue == 1;
        }
        set
        {
            cryptoKey = UnityEngine.Random.Range(1000, 99999);
            int intValue = value ? 1 : 0;
            hiddenValue = intValue ^ cryptoKey;
        }
    }

    public static implicit operator bool(AntiCheatBool encrypted) => encrypted.Value;
    public static implicit operator AntiCheatBool(bool normal) => new AntiCheatBool(normal);
    public override string ToString() => Value.ToString();
}

[Serializable]
public struct AntiCheatString
{
    [SerializeField] private int cryptoKey;
    [SerializeField] private string hiddenValue;

    public AntiCheatString(string value)
    {
        cryptoKey = UnityEngine.Random.Range(1000, 99999);
        hiddenValue = Encrypt(value, cryptoKey);
    }

    public string Value
    {
        get => Decrypt(hiddenValue, cryptoKey);
        set
        {
            cryptoKey = UnityEngine.Random.Range(1000, 99999);
            hiddenValue = Encrypt(value, cryptoKey);
        }
    }

    private static string Encrypt(string text, int key)
    {
        if (string.IsNullOrEmpty(text)) return text;
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(bytes[i] ^ key);
        }
        return Convert.ToBase64String(bytes);
    }

    private static string Decrypt(string text, int key)
    {
        if (string.IsNullOrEmpty(text)) return text;
        try
        {
            byte[] bytes = Convert.FromBase64String(text);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ key);
            }
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return string.Empty;
        }
    }

    public static implicit operator string(AntiCheatString encrypted) => encrypted.Value;
    public static implicit operator AntiCheatString(string normal) => new AntiCheatString(normal);
    public override string ToString() => Value;
}