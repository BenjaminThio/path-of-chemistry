using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Experiment : MonoBehaviour
{
    Database db;
    Dictionary<string, int>[] recipes =
    {
        new Dictionary<string, int>()
        {
            {"Cm", 1},
            {"Og", 1}
        },
        new Dictionary<string, int>()
        {
            {"K", 1},
        },
        new Dictionary<string, int>()
        {
            {"K", 1},
        }
    };

    private void Start()
    {
        db = Database.db;
        UpdateLevel();
    }

    public void React()
    {
        Dictionary<string, int> flaskItemProps = new Dictionary<string, int>();
        foreach (Dictionary<string, object> prop in db.flaskItem)
        {
            if (prop != null)
            {
                if (!flaskItemProps.ContainsKey(Convert.ToString(prop["Item"])))
                {
                    flaskItemProps.Add(Convert.ToString(prop["Item"]), Convert.ToInt32(prop["Quantity"]));
                }
                else
                {
                    flaskItemProps[Convert.ToString(prop["Item"])] += Convert.ToInt32(prop["Quantity"]);
                }
            }
        }
        foreach (Dictionary<string, int> recipe in recipes)
        {
            foreach (KeyValuePair<string, int> material in recipe)
            {
                if (!flaskItemProps.Count.Equals(recipe.Count) || !(flaskItemProps.ContainsKey(material.Key) && flaskItemProps[material.Key].Equals(material.Value)))
                {
                    //Alert
                    print("Pls refer to Chemidex's recipes.");
                    return;
                }
            }
            print("Reaction!");
            LevelUp();
            UpdateLevel();
            return;
        }
    }

    private void LevelUp()
    {
        db.level += 1;
    }

    private void UpdateLevel()
    {
        GameObject.Find("Level").GetComponent<TextMeshProUGUI>().text = Convert.ToString(db.level);
    }
}
