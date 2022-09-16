using System;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private Database db;
    private Dictionary<string, string[]> handItemCategories = new Dictionary<string, string[]>(){
        { "Cylinder Beaker", new string[]{ "AgNo3", "N2H4", "Na3P", "NaH" } },
        { "Flask", new string[]{ "C2H3NaO2", "C18H35NaO2", "H2O", "NaCl" } },
        { "Round Flask", new string[]{ "C3H8O", "HCl", "IO3", "NH3" } },
        { "Beaker", new string[]{ "H2O2", "HNO3", "KI", "Na2S", "NaClO", "NaCN", "NaI" } }
    };

    private void Start()
    {
        db = Database.db;
    }

    public void ChangeItemOnHand()
    {
        if (db.hotbarItem[db.slotNum] != null)
        {
            foreach (string handItemCategory in handItemCategories.Keys)
            {
                foreach (string handItem in handItemCategories[handItemCategory])
                {
                    if (Convert.ToString(db.hotbarItem[db.slotNum - 1]["Item"]) == handItem)
                    {
                        Instantiate(Resources.Load<GameObject>($"Hand/{handItemCategory}"), GameObject.FindGameObjectWithTag("Hand").transform, false);
                        return;
                    }
                }
            }
            Instantiate(Resources.Load<GameObject>($"Hand/Element"), GameObject.FindGameObjectWithTag("Hand").transform, false);
        }
    }
}
