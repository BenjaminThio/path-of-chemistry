using System;
using System.Collections.Generic;
using UnityEngine;

public class CreateCompound : MonoBehaviour
{
    private Database db;
    private readonly Dictionary<KeyValuePair<string, int>, Dictionary<string, int>> recipes = new Dictionary<KeyValuePair<string, int>, Dictionary<string, int>>()
    {
        { 
            new KeyValuePair<string, int>("At", 5), new Dictionary<string, int>()
            {
                {"H", 2},
                {"O", 2}
            }
        }
    };

    void Start()
    {
        db = Database.db;
    }

    public void Create()
    {
        Dictionary<string, int> compoundCreatorItemProps = new Dictionary<string, int>();
        foreach (Dictionary<string, object> prop in db.compoundCreatorItem)
        {
            if (prop != null)
            {
                if (!compoundCreatorItemProps.ContainsKey(Convert.ToString(prop["Item"])))
                {
                    compoundCreatorItemProps.Add(Convert.ToString(prop["Item"]), Convert.ToInt32(prop["Quantity"]));
                }
                else
                {
                    compoundCreatorItemProps[Convert.ToString(prop["Item"])] += Convert.ToInt32(prop["Quantity"]);
                }
            }
        }
        if (compoundCreatorItemProps.Count == 0)
        {
            //Alert
            print("Nothing to create!");
            return;
        }
        foreach (KeyValuePair<string, int> product in recipes.Keys)
        {
            if (compoundCreatorItemProps.Count.Equals(recipes[product].Count) && ContainsProps(recipes[product], compoundCreatorItemProps))
            {
                print($"{product.Key} Created!");
                db.compoundCreatorItem = new Dictionary<string, object>[db.compoundCreatorItem.Length];
                db.compoundCreatorItem[0] = new Dictionary<string, object>()
                {
                    {"Item", product.Key},
                    {"Quantity", product.Value}
                };
                Global.UpdateInventory("Compound Creator", db.compoundCreatorItem);
                StartCoroutine(gameObject.GetComponent<Experience>().AddExp(5));
                return;
            }
        }
        //Alert
        print("Pls refer to the compound's recipes of Chemidex!");
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
}
