using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Experiment : MonoBehaviour
{
    private Database db;
    private Dictionary<string, int>[] recipes =
    {
        new Dictionary<string, int>()
        {
            {"H", 1}
        },
        new Dictionary<string, int>()
        {
            {"H", 2}
        },
        new Dictionary<string, int>()
        {
            {"Cm", 1},
            {"Og", 1}
        },
        new Dictionary<string, int>()
        {
            {"He", 1}
        },
        new Dictionary<string, int>()
        {
            {"O", 5}
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
        if (flaskItemProps.Count == 0)
        {
            //Alert
            print("Nothing to react!");
            return;
        }
        for (int recipeIndex = 0; recipeIndex < recipes.Length; recipeIndex++)
        {
            if (flaskItemProps.Count.Equals(recipes[recipeIndex].Count) && ContainsProps(recipes[recipeIndex], flaskItemProps))
            {
                if (recipeIndex == db.level - 1)
                {
                    print("Reaction!");
                    db.level += 1;
                    UpdateLevel();
                }
                else if (recipeIndex > db.level - 1)
                {
                    //Alert
                    print("Experiement locked.");
                    return;
                }
                db.flaskItem = new Dictionary<string, object>[db.flaskItem.Length];
                Global.UpdateInventory("Flask", db.flaskItem);
                Experience experience = gameObject.GetComponent<Experience>();
                StartCoroutine(experience.AddExp((recipeIndex + 1) * 5));
                return;
            }
        }
        print("Pls refer to the recipes of Chemidex!");
    }

    private bool ContainsProps(Dictionary<string, int> recipe, Dictionary<string, int> props)
    {
        foreach (KeyValuePair<string, int> material in recipe)
        {
            if (!(props.ContainsKey(material.Key) && props[material.Key].Equals(material.Value)))
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateLevel()
    {
        GameObject.Find("Level").GetComponent<TextMeshProUGUI>().text = Convert.ToString(db.level);
    }
}
