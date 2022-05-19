using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class Database
{
    private static readonly object threadLock = new object();
    public static Database database;
    public string hello { get; set; }
    public int[] intArr { get; set; }
    
    public Database()
    {
        hello = "Hello World!";
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

    public Database updateDatabase(Database data)
    {
        database = data;
        return database;
    }
}

public class Recipes : MonoBehaviour
{
    public Database db;
    void Awake()
    {
        db = Load();
        db.hello = "ABCDEFG";
        db.intArr = new int[] {
            5,
            4,
            3,
            2,
            1
        };
        Save();
    }

    private Database Load()
    {
        string filePath = $"{Application.persistentDataPath}/Path Of Chemistry/Data/Saves.json";
        string fileContent = File.ReadAllText(filePath);
        Database data = JsonConvert.DeserializeObject<Database>(fileContent);
        return Database.Data().updateDatabase(data);
    }

    private void Save()
    {
        print(Application.persistentDataPath);
        Database database = Database.Data();
        string directory = $"{Application.persistentDataPath}/Path Of Chemistry/Data";
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Formatting = Formatting.Indented;
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        string data = JsonConvert.SerializeObject(database, settings);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        string filePath = Path.Combine(directory, "Saves.json");
        File.WriteAllText(filePath, data);
    }
}
