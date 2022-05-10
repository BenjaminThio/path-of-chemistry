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
                {"Quantity", 5}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "Mg"},
                {"Quantity", 1}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "He"},
                {"Quantity", 1}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "Na"},
                {"Quantity", 1}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "Cm"},
                {"Quantity", 1}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "Og"},
                {"Quantity", 1}
            }
        },
        {
            new Dictionary<string, object>()
            {
                {"Item", "H"},
                {"Quantity", 1}
            }
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
        var selectedSlotName = EventSystem.current.currentSelectedGameObject.name;
        if (Digitize(selectedSlotName) != slotNum)
        {
            slotNum = Digitize(selectedSlotName);
            UpdateSlot();
        }
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
            if (slotItem[i - 1] != null)
            {
                if (Convert.ToInt32(slotItem[i - 1]["Quantity"]) == 0)
                {
                    slotItem[i - 1] = null;
                    Destroy(GameObject.Find($"Hotbar/Slot ({i})/Item"));
                    continue;
                }
                else if (Convert.ToInt32(slotItem[i - 1]["Quantity"]) > 1)
                {
                    GameObject.Find($"Hotbar/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(slotItem[i - 1]["Quantity"]);
                }
                else
                {
                    GameObject.Find($"Hotbar/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
                }
                GameObject.Find($"Hotbar/Slot ({i})/Item").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Elements/{slotItem[i - 1]["Item"]}");
            }
            else
            {
                Destroy(GameObject.Find($"Hotbar/Slot ({i})/Item"));
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
