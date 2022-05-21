using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Database
{
    public static readonly object threadLock = new object();
    public static Database db;
    public int expLevel = 0;
    public int exp;
    public Dictionary<string, object>[] hotbarItem = {
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        new Dictionary<string, object>()
        {
            {"Item", "O"},
            {"Quantity", 5}
        },
        new Dictionary<string, object>()
        {
            {"Item", "Mg"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "He"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "Na"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "Cm"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "Og"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        null
    };
    public Dictionary<string, object>[] flaskItem = {
        null,
        null,
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        null,
        null,
        null,
        null
    };

    public static Database Load()
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
        return db;
    }

    public static void Save()
    {
        string filePath = $"{Application.persistentDataPath}/Path Of Chemistry/Data/Saves.json";
        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        string data = JsonConvert.SerializeObject(db, settings);
        File.WriteAllText(filePath, data);
    }
}