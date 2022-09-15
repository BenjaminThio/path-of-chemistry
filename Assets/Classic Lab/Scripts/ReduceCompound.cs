using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ReduceCompound : MonoBehaviour
{
    private Database db;

    private void Start()
    {
        db = Database.db;
    }

    public void Reduce()
    {
        if (Global.IsNull(db.compoundReducerOriginalItem))
        {
            Alert.AddAlert("Nothing to reduce!");
            return;
        }
        if (!Global.IsNull(db.compoundReducerReducedItem))
        {
            Alert.AddAlert("Please clean up the \"Reduced Inventory\" before reduce!");
            return;
        }
        Dictionary<string, int> reduced = new Dictionary<string, int>();
        Dictionary<string, Dictionary<string, int>> compounds = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, object>[] convertInventoryFormat = new Dictionary<string, object>[db.compoundReducerReducedItem.Length];
        for (int i = 0; i < Recipe.compounds.Count; i++)
        {
            compounds.Add(Recipe.compounds.ElementAt(i).Key.Key, Recipe.compounds.ElementAt(i).Value);
        }
        foreach (Dictionary<string, object> originalItem in db.compoundReducerOriginalItem)
        {
            string item = Convert.ToString(originalItem["Item"]);
            int quantity = Convert.ToInt32(originalItem["Quantity"]);
            if (compounds.ContainsKey(item))
            {
                foreach (KeyValuePair<string, int> elementAndQuantity in compounds[item])
                {
                    for (int i = 1; i <= quantity; i++)
                    {
                        if (!reduced.ContainsKey(elementAndQuantity.Key))
                        {
                            reduced.Add(elementAndQuantity.Key, elementAndQuantity.Value);
                        }
                        else
                        {
                            reduced[elementAndQuantity.Key] += elementAndQuantity.Value;
                        }
                    }
                }
            }
        }
        db.compoundReducerOriginalItem = new Dictionary<string, object>[db.compoundReducerOriginalItem.Length];
        for (int i = 0; i < db.compoundReducerReducedItem.Length; i++)
        {
            if (i < reduced.Count)
            {
                convertInventoryFormat[i] = new Dictionary<string, object>(){
                    {"Item", reduced.ElementAt(i).Key},
                    {"Quantity", reduced.ElementAt(i).Value}
                };
            }
        }
        db.compoundReducerReducedItem = convertInventoryFormat;
        Global.UpdateInventory("Original", db.compoundReducerOriginalItem);
        Global.UpdateInventory("Reduced", db.compoundReducerReducedItem);
    }
}
