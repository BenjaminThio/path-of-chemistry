using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Hotbar : MonoBehaviour
{
    public static int slotNum = 1;
    public static Dictionary<string, object>[] hotbarItem = {
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        new Dictionary<string, object>()
        {
            {"Item", "O"},
            {"Quantity", 5}
        },
        new Dictionary<string, object>()
        {
            {"Item", "Mg"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "He"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "Na"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "Cm"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "Og"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        null
    };

    private void Awake()
    {
        UpdateSlot();
        UpdateHotbar();
    }

    public void ToggleSlot()
    {
        if (!QuantityHandler.pause)
        {
            var selectedSlotName = EventSystem.current.currentSelectedGameObject.name;
            if (Digitize(selectedSlotName) != slotNum)
            {
                slotNum = Digitize(selectedSlotName);
                UpdateSlot();
            }
        }
    }

    public static int Digitize(string Text)
    {
        char[] charArr = Text.ToCharArray();
        List<char> digitList = new List<char>();
        foreach (char i in charArr)
        {
            if (Char.IsDigit(i))
            {
                digitList.Add(i);
            }
        }
        return Convert.ToInt32(new string(digitList.ToArray()));
    }

    private void UpdateSlot()
    {
        for (int i = 1; i <= GameObject.Find("Hotbar").transform.childCount; i++)
        {
            if (i == slotNum)
            {
                GameObject.Find($"Hotbar/Slot ({i})").GetComponent<Image>().color = Color.cyan;
            }
            else
            {
                GameObject.Find($"Hotbar/Slot ({i})").GetComponent<Image>().color = Color.grey;
            }
        }
    }

    public static void UpdateHotbar()
    {
        for (int i = 1; i <= hotbarItem.Length; i++)
        {
            GameObject.Find($"Hotbar/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
            if (GameObject.Find($"Hotbar/Slot ({i})/Item") == null)
            {
                GameObject item = Instantiate(Resources.Load<GameObject>("Inventory/Item"), GameObject.Find($"Hotbar/Slot ({i})").transform);
                item.name = "Item";
                item.transform.SetAsFirstSibling();
            }
            if (hotbarItem[i - 1] != null)
            {
                if (Convert.ToInt32(hotbarItem[i - 1]["Quantity"]) == 0)
                {
                    hotbarItem[i - 1] = null;
                    Destroy(GameObject.Find($"Hotbar/Slot ({i})/Item"));
                    continue;
                }
                else if (Convert.ToInt32(hotbarItem[i - 1]["Quantity"]) > 1)
                {
                    GameObject.Find($"Hotbar/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(hotbarItem[i - 1]["Quantity"]);
                }
                else
                {
                    GameObject.Find($"Hotbar/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
                }
                GameObject.Find($"Hotbar/Slot ({i})/Item").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Elements/{hotbarItem[i - 1]["Item"]}");
            }
            else
            {
                Destroy(GameObject.Find($"Hotbar/Slot ({i})/Item"));
            }
        }
    }
}
