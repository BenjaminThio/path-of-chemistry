using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Database
{
    private static readonly object threadLock = new object();
    public static Database db;
    public int labIndex = 0;

    public static void Load()
    {
        string directory = $"{Application.persistentDataPath}/Path Of Chemistry/Data";
        string filePath = $"{directory}/Saves.json";
        lock (threadLock)
        {
            if (db == null)
            {
                db = new Database();
            }
        }
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        if (!File.Exists(filePath))
        {
            Save();
        }
        string fileContent = File.ReadAllText(filePath);
        Database data = JsonConvert.DeserializeObject<Database>(fileContent);
        db = data;
    }

    public static void Save()
    {
        string filePath = $"{Application.persistentDataPath}/Path Of Chemistry/Data/Preview.json";
        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        string data = JsonConvert.SerializeObject(db, settings);
        File.WriteAllText(filePath, data);
    }

    public static string Log(object rawData)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        string newData = JsonConvert.SerializeObject(rawData, settings);
        return newData;
    }
}