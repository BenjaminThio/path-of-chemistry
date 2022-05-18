using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class Recipes : MonoBehaviour
{
    void Awake()
    {
        print(Application.persistentDataPath);
        string gameDataPath = $"{Application.persistentDataPath}/Path Of Chemistry";
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Formatting = Formatting.Indented;
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        foreach (string i in new string[]
        {
            "Data",
            "Freeze Data"
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
            File.WriteAllText(filePath, data);
        }
    }
}
