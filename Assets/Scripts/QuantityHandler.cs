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
            {"Quantity", 64}
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

    public void UpdateQuantity(float value)
    {
        GameObject.Find("Quantity Handler/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(Mathf.Floor(value));
    }

    public void AddSliderValue()
    {
        if (GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().value + 1 <= GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().maxValue)
        {
            GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().value += 1;
        }
    }

    public void RemoveSliderValue()
    {
        if (GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().value - 1 >= GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().minValue)
        {
            GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().value -= 1;
        }
    }

    public void MaxSliderValue()
    {
        GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().value = GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().maxValue;
    }

    public void Add()
    {
        if (Hotbar.hotbarItem[Hotbar.slotNum - 1] != null)
        {
            if (Convert.ToInt32(Hotbar.hotbarItem[Hotbar.slotNum - 1]["Quantity"]) > 1)
            {
                GameObject quantityHandler = Instantiate(Resources.Load<GameObject>("Inventory/Quantity Handler"), GameObject.Find("Canvas").transform, false);
                quantityHandler.name = "Quantity Handler";
                quantityHandler.transform.GetChild(5).GetComponent<Slider>().maxValue = Convert.ToInt32(Hotbar.hotbarItem[Hotbar.slotNum - 1]["Quantity"]);
            }
            else
            {
                RepeatAddItem(1);
            }
            UpdateFlask();
            UpdateHotbar();
        }
    }

    public void Done()
    {
        RepeatAddItem(Convert.ToInt32(Mathf.Floor(GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().value)));
        UpdateFlask();
        UpdateHotbar();
        Destroy(GameObject.Find("Quantity Handler"));
    }

    private void RepeatAddItem(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            AddItem();
        }
    }
    private void AddItem()
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
        if (allFlaskItemNames.Contains(Convert.ToString(Hotbar.hotbarItem[Hotbar.slotNum - 1]["Item"])))
        {
            for (int i = 0; i < flaskItem.Count; i++)
            {
                if (Convert.ToString(Hotbar.hotbarItem[Hotbar.slotNum - 1]["Item"]) == allFlaskItemNames[i] && Convert.ToInt32(flaskItem[i]["Quantity"]) < 64)
                {
                    flaskItem[i]["Quantity"] = Convert.ToInt32(flaskItem[i]["Quantity"]) + 1;
                    Hotbar.hotbarItem[Hotbar.slotNum - 1]["Quantity"] = Convert.ToInt32(Hotbar.hotbarItem[Hotbar.slotNum - 1]["Quantity"]) - 1;
                    return;
                }
            }
            for (int i = 0; i < flaskItem.Count; i++)
            {
                if (Convert.ToString(Hotbar.hotbarItem[Hotbar.slotNum - 1]["Item"]) == allFlaskItemNames[i] && Convert.ToInt32(flaskItem[i]["Quantity"]) >= 64)
                {
                    ItemNotFound();
                    return;
                }
            }
        }
        else
        {
            ItemNotFound();
        }
    }

    private void ItemNotFound()
    {
        if (flaskItem.Contains(null))
        {
            for (int i = 0; i < flaskItem.Count; i++)
            {
                if (flaskItem[i] == null)
                {
                    flaskItem[i] = new Dictionary<string, object>()
                        {
                            {"Item", Hotbar.hotbarItem[Hotbar.slotNum - 1]["Item"]},
                            {"Quantity", 1}
                        };
                    Hotbar.hotbarItem[Hotbar.slotNum - 1]["Quantity"] = Convert.ToInt32(Hotbar.hotbarItem[Hotbar.slotNum - 1]["Quantity"]) - 1;
                    return;
                }
            }
        }
        else
        {
            //Alert
        }
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
                item.transform.SetAsFirstSibling();
            }
            if (flaskItem[i - 1] != null)
            {
                if (Convert.ToInt32(flaskItem[i - 1]["Quantity"]) == 0)
                {
                    flaskItem[i - 1] = null;
                    Destroy(GameObject.Find($"Flask/Inventory/Slot ({i})/Item"));
                    continue;
                }
                else if (Convert.ToInt32(flaskItem[i - 1]["Quantity"]) > 1)
                {
                    GameObject.Find($"Flask/Inventory/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(flaskItem[i - 1]["Quantity"]);
                }
                else
                {
                    GameObject.Find($"Flask/Inventory/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
                }
                GameObject.Find($"Flask/Inventory/Slot ({i})/Item").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Elements/{flaskItem[i - 1]["Item"]}");
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
                item.transform.SetAsFirstSibling();
            }
            if (Hotbar.hotbarItem[i - 1] != null)
            {
                if (Convert.ToInt32(Hotbar.hotbarItem[i - 1]["Quantity"]) == 0)
                {
                    Hotbar.hotbarItem[i - 1] = null;
                    Destroy(GameObject.Find($"Hotbar/Slot ({i})/Item"));
                    continue;
                }
                else if (Convert.ToInt32(Hotbar.hotbarItem[i - 1]["Quantity"]) > 1)
                {
                    GameObject.Find($"Hotbar/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(Hotbar.hotbarItem[i - 1]["Quantity"]);
                }
                else
                {
                    GameObject.Find($"Hotbar/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
                }
                GameObject.Find($"Hotbar/Slot ({i})/Item").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Elements/{Hotbar.hotbarItem[i - 1]["Item"]}");
            }
            else
            {
                Destroy(GameObject.Find($"Hotbar/Slot ({i})/Item"));
            }
        }
    }
}
