using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelHandler : MonoBehaviour
{
    public static void UpdateLevel()
    {
        if (Database.db.level <= Recipe.experiments.Length)
        {
            GameObject.Find("Level").GetComponent<TextMeshProUGUI>().text = Convert.ToString(Database.db.level);
        }
        else
        {
            GameObject.Find("Level").GetComponent<TextMeshProUGUI>().text = Convert.ToString("Completed");
            GameObject.Find("Guide").GetComponent<TextMeshProUGUI>().text = "Under Construction!";
            return;
        }
        string info = "";
        Dictionary<string, int> recipeItems = new Dictionary<string, int>();
        recipeItems = Recipe.experiments[Database.db.level - 1];
        for (int itemNum = 0; itemNum < recipeItems.Count; itemNum++)
        {
            KeyValuePair<string, int> item = recipeItems.ElementAt(itemNum);
            info += $"{item.Key} x {item.Value}";
            if (itemNum < recipeItems.Count - 1)
            {
                info += " + ";
            }
        }
        GameObject.Find("Guide").GetComponent<TextMeshProUGUI>().text = info;
    }
}
