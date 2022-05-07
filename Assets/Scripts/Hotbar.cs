using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Hotbar : MonoBehaviour
{
    public static string slotName = "Slot (1)";
    public static List<Dictionary<string, object>> slotItem = new List<Dictionary<string, object>>()
    {
        {
            new Dictionary<string, object>()
            {
                {"Item", "H"},
                {"Quantity", 1}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "O"},
                {"Quantity", 3}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "Mg"},
                {"Quantity", 5}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "He"},
                {"Quantity", 7}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "Na"},
                {"Quantity", 9}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "Cm"},
                {"Quantity", 11}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "Og"},
                {"Quantity", 13}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "H"},
                {"Quantity", 15}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "N"},
                {"Quantity", 17}
            }
        }
    };

    private void Start()
    {
        UpdateHotbar();
    }

    public void UpdateSlot()
    {
        var selectedSlotName = EventSystem.current.currentSelectedGameObject.name;
        if (selectedSlotName != slotName)
        {
            slotName = selectedSlotName;
            UpdateHotbar();
        }
    }

    private void UpdateHotbar()
    {
        for (int i = 1; i <= GameObject.Find("Hotbar").transform.childCount; i++)
        {
            string Item = Convert.ToString(slotItem[i - 1]["Item"]);
            print(Item);
            GameObject.Find($"Hotbar/Slot ({i})/Item").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Elements/{Item}");
            if ($"Slot ({i})" == slotName)
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
