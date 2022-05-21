using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Hotbar : MonoBehaviour
{
    public Database db;
    public static int slotNum = 1;
    private void Awake()
    {
        db = Database.Load();
        UpdateSlot();
        QuantityHandler.UpdateInventory("Hotbar", db.hotbarItem);
    }

    public void Save()
    {
        Database.Save();
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
            if (char.IsDigit(i))
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
}
