using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Recipe : MonoBehaviour
{
    private int selectedSlotNum = 1;
    public static readonly Dictionary<KeyValuePair<string, int>, Dictionary<string, int>> compounds = new Dictionary<KeyValuePair<string, int>, Dictionary<string, int>>()
    {
        {
            new KeyValuePair<string, int>("At", 5),
            new Dictionary<string, int>()
            {
                {"H", 2},
                {"O", 2}
            }
        }
    };
    public static readonly Dictionary<string, int>[] experiments =
    {
        new Dictionary<string, int>()
        {
            {"H", 1}
        },
        new Dictionary<string, int>()
        {
            {"H", 2}
        },
        new Dictionary<string, int>()
        {
            {"Cm", 1},
            {"Og", 1}
        },
        new Dictionary<string, int>()
        {
            {"He", 1}
        },
        new Dictionary<string, int>()
        {
            {"O", 5}
        }
    };

    private void Start()
    {
        for (int i = 1; i <= experiments.Length; i++)
        {
            GameObject recipe = Instantiate(Resources.Load<GameObject>("UI/Recipe Slot"), GameObject.Find("Recipe Interface/Scroll View/Viewport/Content").transform);
            recipe.name = $"Recipe Slot ({i})";
            GameObject.Find($"Recipe Slot ({i})").GetComponent<Button>().onClick.AddListener(ToggleRecipeSlot);
            GameObject.Find($"Recipe Slot ({i})/Text").GetComponent<TextMeshProUGUI>().text = Convert.ToString(i);
        }
        UpdateRecipeSlot();
    }

    public void ToggleRecipeSlot()
    {
        if (!QuantityHandler.pause)
        {
            string selectedSlotName = EventSystem.current.currentSelectedGameObject.name;
            int digitizedSelectedSlotName = Global.Digitize(selectedSlotName);
            if (digitizedSelectedSlotName != selectedSlotNum)
            {
                selectedSlotNum = digitizedSelectedSlotName;
                UpdateRecipeSlot();
            }
        }
    }

    public void UpdateRecipeSlot()
    {
        for (int i = 1; i <= experiments.Length; i++)
        {
            if (i == selectedSlotNum)
            {
                GameObject.Find($"Recipe Slot ({i})").GetComponent<Image>().color = Color.cyan;
            }
            else
            {
                GameObject.Find($"Recipe Slot ({i})").GetComponent<Image>().color = Color.grey;
            }
        }
        string info = "";
        for (int itemNum = 0; itemNum < experiments[selectedSlotNum - 1].Count; itemNum++)
        {
            KeyValuePair<string, int> item = experiments[selectedSlotNum - 1].ElementAt(itemNum);
            info += $"{item.Key} x {item.Value}";
            if (itemNum < experiments[selectedSlotNum - 1].Count - 1)
            {
                info += " + ";
            }
        }
        GameObject.Find("Recipe Interface/Info").GetComponent<TextMeshProUGUI>().text = info;
    }
}
