using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            "magenta", new string[]
            {
                "AgNO3"
            }
        },
        {
            "blue", new string[]
            {
                "C2H3NaO2",
                "H2O"
            }
        },
        {
            "yellow", new string[]
            {
                "C3H8O",
                "KI",
                "Na3P",
                "NaCN",
                "NH3"
            }
        },
        {
            "red", new string[]
            {
                "C18H35NaO2",
                "NaCl"
            }
        },
        {
            "green", new string[]
            {
                "H2O2",
                "HNO3"
            }
        },
        {
            "gold", new string[]
            {
                "HCl",
                "IO3"
            }
        },
        {
            "darkGreen", new string[]
            {
                "N2H4"
            }
        },
        {
            "purple", new string[]
            {
                "Na2S"
            }
        },
        {
            "orange", new string[]
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
                    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
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
                        GameObject liquid = Instantiate(Resources.Load<GameObject>($"Liquids/{textInfo.ToTitleCase(color)}"), GameObject.Find($"Cylinder Beaker ({a + 1})").transform, false);
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
