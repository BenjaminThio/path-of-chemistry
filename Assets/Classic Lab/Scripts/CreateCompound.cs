using System.Collections.Generic;
using UnityEngine;

public class CreateCompound : MonoBehaviour
{
    private Database db;

    void Start()
    {
        db = Database.db;
    }

    public void Create()
    {
        Dictionary<string, int> itemProps = Global.CreateVirtualProps(db.compoundCreatorItem);
        if (itemProps.Count == 0)
        {
            //Alert
            print("Nothing to create!");
            return;
        }
        foreach (KeyValuePair<string, int> product in Recipe.compounds.Keys)
        {
            if (itemProps.Count.Equals(Recipe.compounds[product].Count) && Global.ContainsProps(Recipe.compounds[product], itemProps))
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
}
