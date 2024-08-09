using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class GameData
{
    public string PlayerName = "";
    public int Gold;
    public int Diamond;
    public int Ticket;
    public int Mileage;
    public bool NicknameCheck;
    public bool StageChoiceTutorialCheck;
    public bool DeckUITutorialCheck;
    public bool Game1TutorialCheck;
    public bool Game2TutorialCheck;
    public bool Game3TutorialCheck;
    public bool Game4TutorialCheck;
    
    public List<int> ObtainedGachaIDs = new List<int>();
    public List<Tuple<int,int>> ItemList = new List<Tuple<int, int>>();
}

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    
    private readonly string keyFileName = "encryptionKey.dat";
    private readonly string ivFileName = "encryptionIV.dat";
    private byte[] encryptionKey;
    private byte[] encryptionIV;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadEncryptionKeyAndIv();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadEncryptionKeyAndIv()
    {
        string keyPath = Path.Combine(Application.persistentDataPath, keyFileName);
        string ivPath = Path.Combine(Application.persistentDataPath, ivFileName);

        if (File.Exists(keyPath) && File.Exists(ivPath))
        {
            encryptionKey = File.ReadAllBytes(keyPath);
            encryptionIV = File.ReadAllBytes(ivPath);
        }
        else
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();
                encryptionKey = aes.Key;
                encryptionIV = aes.IV;
            }

            File.WriteAllBytes(keyPath, encryptionKey);
            File.WriteAllBytes(ivPath, encryptionIV);
        }
    }

    public void SaveFile<T>(string fileName, T data)
    {
        string json = JsonConvert.SerializeObject(data);
        string encrypt = Encrypt(json);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, encrypt);
    }

    public T LoadFile<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (!File.Exists(path))
        {
            return default(T);
        }
        string encrypt = File.ReadAllText(path);
        string decrypt = Decrypt(encrypt);
        return JsonConvert.DeserializeObject<T>(decrypt);
    }

    private string Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = encryptionKey;
        aes.IV = encryptionIV;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using MemoryStream ms = new MemoryStream();
        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (StreamWriter sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    private string Decrypt(string cipherText)
    {
        byte[] buffer = Convert.FromBase64String(cipherText);
        using Aes aes = Aes.Create();
        aes.Key = encryptionKey;
        aes.IV = encryptionIV;

        ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
        using MemoryStream ms = new MemoryStream(buffer);
        using CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read);
        using StreamReader sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}
