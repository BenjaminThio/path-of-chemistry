using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuantityHandler : MonoBehaviour
{
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
            Hotbar.flaskItem[0] = new Dictionary<string, object>()
            {
                {"Item", Hotbar.slotItem[Hotbar.slotNum - 1]["Item"]},
                {"Quantity", Hotbar.slotItem[Hotbar.slotNum - 1]["Quantity"]}
            };
            Hotbar.slotItem[0] = null;
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
            if (Hotbar.flaskItem[i - 1] != null)
            {
                GameObject.Find($"Flask/Inventory/Slot ({i})/Item").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Elements/{Hotbar.flaskItem[i - 1]["Item"]}");
                if (Convert.ToInt32(Hotbar.flaskItem[i - 1]["Quantity"]) == 1)
                {
                    GameObject.Find($"Flask/Inventory/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(Hotbar.flaskItem[i - 1]["Quantity"]);
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
            if (Hotbar.slotItem[i - 1] == null)
            {
                Destroy(GameObject.Find($"Hotbar/Slot ({i})/Item"));
            }
            else
            {
                /*
                if (GameObject.Find($"Hotbar/Slot ({i})/Item") == null)
                {
                    GameObject item = Instantiate(Resources.Load<GameObject>("Hotbar/Item"), GameObject.Find($"Hotbar/Slot ({i})").transform);
                    item.name = "Item";
                }
                */
                GameObject.Find($"Hotbar/Slot ({i})/Item").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Elements/{Convert.ToString(Hotbar.slotItem[i - 1]["Item"])}");
            }
            if (Hotbar.slotItem[i - 1] == null || Convert.ToInt32(Hotbar.slotItem[i - 1]["Quantity"]) == 1)
            {
                Destroy(GameObject.Find($"Hotbar/Slot ({i})/Quantity"));
            }
            else
            {
                /*
                if (GameObject.Find($"Hotbar/Slot ({i})/Quantity") == null)
                {
                    GameObject quantity = Instantiate(Resources.Load<GameObject>("Hotbar/Quantity"), GameObject.Find($"Hotbar/Slot ({i})").transform);
                    quantity.name = "Quantity";
                }
                */
                GameObject.Find($"Hotbar/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(Hotbar.slotItem[i - 1]["Quantity"]);
            }
            if (i == Hotbar.slotNum)
            {
                GameObject.Find($"Hotbar/Slot ({i})").GetComponent<Image>().color = Color.cyan;
            }
            else
            {
                GameObject.Find($"Hotbar/Slot ({i})").GetComponent<Image>().color = Color.grey;
            }
        }
    }
}
