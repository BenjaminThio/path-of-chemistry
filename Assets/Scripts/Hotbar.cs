using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Hotbar : MonoBehaviour
{
    public static int slotNum = 1;
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
        null
    };
    public static List<Dictionary<string, object>> flaskItem = new List<Dictionary<string, object>>()
    {
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null
    };

    private void Awake()
    {
        UpdateHotbar();
    }

    public void UpdateSlot()
    {
        var selectedSlotName = EventSystem.current.currentSelectedGameObject.name;
        if (Digitize(selectedSlotName) != slotNum)
        {
            slotNum = Digitize(selectedSlotName);
            UpdateHotbar();
        }
    }

    private void UpdateHotbar()
    {
        for (int i = 1; i <= GameObject.Find("Hotbar").transform.childCount; i++)
        {
            if (slotItem[i - 1] == null)
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
                GameObject.Find($"Hotbar/Slot ({i})/Item").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Elements/{Convert.ToString(slotItem[i - 1]["Item"])}");
            }
            if (slotItem[i - 1] == null || Convert.ToInt32(slotItem[i - 1]["Quantity"]) == 1)
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
                GameObject.Find($"Hotbar/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(slotItem[i - 1]["Quantity"]);
            }
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

    private int Digitize(string Text)
    {
        List<char> charArr = new List<char>(Text);
        List<char> digitList = new List<char>();
        for (int i = 0; i <= charArr.Count - 1; i++)
        {
            if (Char.IsDigit(charArr[i]))
            {
                digitList.Add(charArr[i]);
            }
        }
        return Convert.ToInt32(new string(digitList.ToArray()));
    }
}
