using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class Database
{
    private static readonly object threadLock = new object();
    private static Database database;
    public int[] intArr { get; set; }
    
    public Database()
    {
        intArr = new int[]
        {
            1,
            1,
            1
        };
    }

    public static Database Data()
    {
        lock (threadLock)
        {
            if (database == null)
            {
                database = new Database();
            }
            return database;
        }
    }

    public void updateDatabase(Database data)
    {
        database = data;
    }
}

public class Recipes : MonoBehaviour
{
    void Awake()
    {
        Save();
    }

    private void Save()
    {
        print(Application.persistentDataPath);
        Database database = Database.Data();
        string gameDataPath = $"{Application.persistentDataPath}/Path Of Chemistry";
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Formatting = Formatting.Indented;
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        var json = JsonConvert.SerializeObject(database, settings);
        foreach (string i in new string[]
        {
            "Data"
            //"Freeze Data"
        })
        {
            string directory = $"{gameDataPath}/{i}";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filePath = Path.Combine(directory, "Saves.json");
            string data = JsonConvert.SerializeObject(new int[]
            {
                1,
                2,
                3,
                4,
                5
            },
            settings);
            File.WriteAllText(filePath, json);
        }
    }
}
