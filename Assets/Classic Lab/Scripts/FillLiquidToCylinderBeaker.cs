using System;
using System.Collections.Generic;
using UnityEngine;

public class FillLiquidToCylinderBeaker : MonoBehaviour
{
    private Database db;
    private float fillRange = 1.28f;
    private float fillStartPoint = -0.44f;
    private int maxItemQuantity = 64;
    private Dictionary<string, string[]> liquidColors = new Dictionary<string, string[]>()
    {
        {
            "Magenta", new string[]
            {
                "AgNO3"
            }
        },
        {
            "Blue", new string[]
            {
                "C2H3NaO2",
                "H2O"
            }
        },
        {
            "Yellow", new string[]
            {
                "C3H8O",
                "KI",
                "Na3P",
                "NaCN",
                "NH3"
            }
        },
        {
            "Red", new string[]
            {
                "C18H35NaO2",
                "NaCl"
            }
        },
        {
            "Green", new string[]
            {
                "H2O2",
                "HNO3"
            }
        },
        {
            "Gold", new string[]
            {
                "HCl",
                "IO3"
            }
        },
        {
            "Dark Green", new string[]
            {
                "N2H4"
            }
        },
        {
            "Purple", new string[]
            {
                "Na2S"
            }
        },
        {
            "Orange", new string[]
            {
                "NaClO",
                "NaH",
                "NaI"
            }
        }
    };

    private void Start()
    {
        db = Database.db;
        FillLiquid();
    }

    public void FillLiquid()
    {
        for (int a = 0; a < db.flaskItem.Length; a++)
        {
            if (db.flaskItem[a] != null)
            {
                string itemName = Convert.ToString(db.flaskItem[a]["Item"]);
                int quantity = Convert.ToInt32(db.flaskItem[a]["Quantity"]);
                float fill = fillRange * quantity / maxItemQuantity;
                foreach (string color in liquidColors.Keys)
                {
                    string[] elements = new string[ReadOnly.elements.Length];
                    for (int b = 0; b < ReadOnly.elements.Length; b++)
                    {
                        elements[b] = Convert.ToString(ReadOnly.elements[b]["symbol"]);
                    }
                    if (GameObject.Find($"Cylinder Beaker ({a + 1})/Liquid") != null)
                    {
                        Destroy(GameObject.Find($"Cylinder Beaker ({a + 1})/Liquid"));
                    }
                    if (Global.IsItemExist(itemName, elements))
                    {
                        GameObject liquid = Instantiate(Resources.Load<GameObject>("Liquids/Black"), GameObject.Find($"Cylinder Beaker ({a + 1})").transform, false);
                        liquid.name = "Liquid";
                        liquid.GetComponent<MeshRenderer>().material.SetFloat("Fill", fillStartPoint + fill);
                        break;
                    }
                    else if (Global.IsItemExist(itemName, liquidColors[color]))
                    {
                        GameObject liquid = Instantiate(Resources.Load<GameObject>($"Liquids/{color}"), GameObject.Find($"Cylinder Beaker ({a + 1})").transform, false);
                        liquid.name = "Liquid";
                        liquid.GetComponent<MeshRenderer>().material.SetFloat("Fill", fillStartPoint + fill);
                        break;
                    }
                }
            }
            else
            {
                Destroy(GameObject.Find($"Cylinder Beaker ({a + 1})/Liquid"));
            }
        }
    }
}
