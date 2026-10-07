using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SaveProtector
{
    private static readonly string salt = "ShiroSaveSystem_SecretHashKey_2026!"; 

    public static void GenerateHashForFile(string filePath)
    {
        if (!File.Exists(filePath)) return;

        try
        {
            string fileContent = File.ReadAllText(filePath);
            string hash = CalculateSHA256(fileContent + salt);
            
            File.WriteAllText(filePath + ".hash", hash);
        }
        catch (Exception e)
        {
            Debug.LogError($"[Anti-Cheat] Failed to generate hash: {e.Message}");
        }
    }

    public static bool IsSaveFileValid(string filePath)
    {
        string hashPath = filePath + ".hash";

        if (!File.Exists(filePath)) return true; 
        if (!File.Exists(hashPath)) return false;

        try
        {
            string currentContent = File.ReadAllText(filePath);
            string savedHash = File.ReadAllText(hashPath).Trim();

            string calculatedHash = CalculateSHA256(currentContent + salt);

            return string.Equals(savedHash, calculatedHash, StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static string CalculateSHA256(string text)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
