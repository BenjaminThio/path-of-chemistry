using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class Database
{
    public static Database db;
    public string hello = "Hello World!";
    public int[] intArr = new int[]
    {
        1, 2, 3, 4, 5
    };

    public static Database UpdateDatabase(Database data)
    {
        db = data;
        return db;
    }
}

public class DataStore : MonoBehaviour
{
    public Database db;
    private void Awake()
    {
        db = Load();
        db.hello = "Gay!";
        db.intArr = new int[] {
            7, 17, 27, 37, 47, 57, 67, 77
        };
        Save();
    }

    public void Click()
    {
        db.hello = "Edmund";
        Save();
    }
    
    public static Database Load()
    {
        string directory = $"{Application.persistentDataPath}/Path Of Chemistry/Data";
        string filePath = $"{directory}/Saves.json";
        if (Database.db == null)
        {
            Database.db = new Database();
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
        return Database.UpdateDatabase(data);
    }

    public static void Save()
    {
        string filePath = $"{Application.persistentDataPath}/Path Of Chemistry/Data/Saves.json";
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Formatting = Formatting.Indented;
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        string data = JsonConvert.SerializeObject(Database.db, settings);
        File.WriteAllText(filePath, data);
    }
}
