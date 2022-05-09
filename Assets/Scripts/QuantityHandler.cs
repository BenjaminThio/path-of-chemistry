using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuantityHandler : MonoBehaviour
{
    public static List<Dictionary<string, object>> flaskItem = new List<Dictionary<string, object>>()
    {
        null,
        null,
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 5}
        },
        null,
        null,
        null,
        null,
        null,
        null
    };

    void Awake()
    {
        UpdateFlask();
    }

    public void Add()
    {
        if (Hotbar.slotItem[Hotbar.slotNum - 1] == null)
        {
            return;
        }
        if (Convert.ToInt32(Hotbar.slotItem[Hotbar.slotNum - 1]["Quantity"]) > 1)
        {
            GameObject quantityHandler = Instantiate(Resources.Load<GameObject>("Inventory/Quantity Handler"), GameObject.Find("Canvas").transform, false);
            quantityHandler.name = "Quantity Handler";
            quantityHandler.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(Hotbar.slotItem[Hotbar.slotNum - 1]["Quantity"]);
        }
        else
        {
            List<string> allFlaskItemNames = new List<string>();
            foreach (var i in flaskItem)
            {
                if (i != null)
                {
                    allFlaskItemNames.Add(Convert.ToString(i["Item"]));
                }
                else
                {
                    allFlaskItemNames.Add(null);
                }
            }
            if (allFlaskItemNames.Contains(Convert.ToString(Hotbar.slotItem[Hotbar.slotNum - 1]["Item"])))
            {
                for (int i = 0; i < flaskItem.Count; i++)
                {
                    if (Convert.ToString(Hotbar.slotItem[Hotbar.slotNum - 1]["Item"]) == allFlaskItemNames[i])
                    {
                        flaskItem[i]["Quantity"] = Convert.ToInt32(flaskItem[i]["Quantity"]) + Convert.ToInt32(Hotbar.slotItem[Hotbar.slotNum - 1]["Quantity"]);
                        Hotbar.slotItem[Hotbar.slotNum - 1] = null;
                        break;
                    }
                }
            }
            else if (flaskItem.Contains(null))
            {
                for (int i = 0; i < flaskItem.Count; i++)
                {
                    if (flaskItem[i] == null)
                    {
                        flaskItem[i] = new Dictionary<string, object>()
                        {
                            { "Item", Hotbar.slotItem[Hotbar.slotNum - 1]["Item"] },
                            { "Quantity", Hotbar.slotItem[Hotbar.slotNum - 1]["Quantity"] }
                        };
                        Hotbar.slotItem[Hotbar.slotNum - 1] = null;
                        break;
                    }
                }
            }
            else
            {
                //Alert
            }
        }
        UpdateFlask();
        UpdateHotbar();
    }

    public void Done()
    {
        Destroy(GameObject.Find("Quantity Handler"));
    }

    private void UpdateFlask()
    {
        for (int i = 1; i <= GameObject.Find("Flask/Inventory").transform.childCount; i++)
        {
            GameObject.Find($"Flask/Inventory/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
            if (GameObject.Find($"Flask/Inventory/Slot ({i})/Item") == null)
            {
                GameObject item = Instantiate(Resources.Load<GameObject>("Inventory/Item"), GameObject.Find($"Flask/Inventory/Slot ({i})").transform);
                item.name = "Item";
            }
            if (flaskItem[i - 1] != null)
            {
                GameObject.Find($"Flask/Inventory/Slot ({i})/Item").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Elements/{flaskItem[i - 1]["Item"]}");
                if (Convert.ToInt32(flaskItem[i - 1]["Quantity"]) > 1)
                {
                    GameObject.Find($"Flask/Inventory/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(flaskItem[i - 1]["Quantity"]);
                }
                else
                {
                    GameObject.Find($"Flask/Inventory/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
                }
            }
            else
            {
                Destroy(GameObject.Find($"Flask/Inventory/Slot ({i})/Item"));
            }
        }
    }
    private void UpdateHotbar()
    {
        for (int i = 1; i <= GameObject.Find("Hotbar").transform.childCount; i++)
        {
            GameObject.Find($"Hotbar/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
            if (GameObject.Find($"Hotbar/Slot ({i})/Item") == null)
            {
                GameObject item = Instantiate(Resources.Load<GameObject>("Inventory/Item"), GameObject.Find($"Hotbar/Slot ({i})").transform);
                item.name = "Item";
            }
            if (Hotbar.slotItem[i - 1] != null)
            {
                GameObject.Find($"Hotbar/Slot ({i})/Item").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Elements/{Hotbar.slotItem[i - 1]["Item"]}");
                if (Convert.ToInt32(Hotbar.slotItem[i - 1]["Quantity"]) > 1)
                {
                    GameObject.Find($"Hotbar/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(Hotbar.slotItem[i - 1]["Quantity"]);
                }
                else
                {
                    GameObject.Find($"Hotbar/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
                }
            }
            else
            {
                Destroy(GameObject.Find($"Hotbar/Slot ({i})/Item"));
            }
        }
    }
}
