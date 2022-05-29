using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Experiment : MonoBehaviour
{
    private Database db;

    private void Start()
    {
        db = Database.db;
        UpdateLevel();
    }

    public void React()
    {
        Dictionary<string, int> itemProps = Global.CreateVirtualProps(db.flaskItem);
        if (itemProps.Count == 0)
        {
            //Alert
            print("Nothing to react!");
            return;
        }
        for (int recipeIndex = 0; recipeIndex < Recipe.experiments.Length; recipeIndex++)
        {
            if (itemProps.Count.Equals(Recipe.experiments[recipeIndex].Count) && Global.ContainsProps(Recipe.experiments[recipeIndex], itemProps))
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
                StartCoroutine(gameObject.GetComponent<Experience>().AddExp((recipeIndex + 1) * 5));
                return;
            }
        }
        //Alert
        print("Pls refer to the experiment's recipes of Chemidex!");
    }

    private void UpdateLevel()
    {
        GameObject.Find("Level").GetComponent<TextMeshProUGUI>().text = Convert.ToString(db.level);
    }
}
