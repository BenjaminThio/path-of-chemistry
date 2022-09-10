using System;
using System.Collections.Generic;
using UnityEngine;


public class Experiment : MonoBehaviour
{
    private Database db;

    private void Start()
    {
        db = Database.db;
    }

    public void React()
    {
        Dictionary<string, int> itemProps = Global.CreateVirtualProps(db.flaskItem);
        if (itemProps.Count == 0)
        {
            Alert.AddAlert("Nothing to react!");
            return;
        }
        for (int recipeIndex = 0; recipeIndex < Recipe.experiments.Length; recipeIndex++)
        {
            if (itemProps.Count.Equals(Recipe.experiments[recipeIndex].Count) && Global.ContainsProps(Recipe.experiments[recipeIndex], itemProps))
            {
                if (recipeIndex == db.level - 1)
                {
                    //print("Reaction!");
                    GameObject.FindGameObjectWithTag("Reaction").GetComponent<Reaction>().PlayPourAnimation();
                    db.level += 1;
                    LevelHandler.UpdateLevel();
                }
                else if (recipeIndex > db.level - 1)
                {
                    Alert.AddAlert("Experiement locked.");
                    return;
                }
                db.flaskItem = new Dictionary<string, object>[db.flaskItem.Length];
                Global.UpdateInventory("Flask", db.flaskItem);
                StartCoroutine(gameObject.GetComponent<Experience>().AddExp((recipeIndex + 1) * 5));
                return;
            }
        }
        Alert.AddAlert("Please refer to the experiment's recipes of Chemidex!");
    }
}
