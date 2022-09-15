using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompoundReducer : MonoBehaviour
{
    private Database db;

    private void Start()
    {
        db = Database.db;
        GameObject.FindGameObjectWithTag("Clear").GetComponent<Button>().onClick.AddListener(Clear);
    }

    private void Clear()
    {
        if (!Global.IsNull(db.compoundReducerReducedItem))
        {
            db.compoundReducerReducedItem = new Dictionary<string, object>[db.compoundReducerReducedItem.Length];
            Global.UpdateInventory("Reduced", db.compoundReducerReducedItem);
        }
        else
        {
            Alert.AddAlert("Nothing to clear.");
        }
    }
}
