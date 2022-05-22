using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuantityHandler : MonoBehaviour
{
    private Database db;
    public static bool pause = false;
    public static int selectedSlotNum;

    private void Start()
    {
        db = Database.db;
        Inventory.UpdateInventory("Flask", db.flaskItem);
    }

    public void Save()
    {
        Database.Save();
    }

    public void UpdateQuantity(float value)
    {
        GameObject.Find("Quantity Handler/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(Mathf.Floor(value));
        IdentifyQuantity(Mathf.Floor(value));
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

    private void IdentifyQuantity(float quantity)
    {
        if (quantity == 0)
        {
            GameObject.Find("Quantity Handler/Done/Text").GetComponent<TextMeshProUGUI>().text = "Cancel";
        }
        else
        {
            GameObject.Find("Quantity Handler/Done/Text").GetComponent<TextMeshProUGUI>().text = "Done";
        }
    }

    public void TransferHotbarToFlask()
    {
        Estimation(db.hotbarItem, db.flaskItem, db.slotNum);
    }

    public void TransferFlaskToHotbar()
    {
        if (!pause)
        {
            selectedSlotNum = Inventory.GetSlotNum(EventSystem.current.currentSelectedGameObject.name);
            if (db.flaskItem[selectedSlotNum - 1] != null)
            {
                GameObject.Find($"Flask/Slot ({selectedSlotNum})").GetComponent<Image>().color = Color.cyan;
            }
        }
        Estimation(db.flaskItem, db.hotbarItem, selectedSlotNum);
    }

    private void Estimation(Dictionary<string, object>[] src, Dictionary<string, object>[] dst, int slotNum)
    {
        if (!pause)
        {
            if (src[slotNum - 1] != null)
            {
                if (Convert.ToInt32(src[slotNum - 1]["Quantity"]) > 1)
                {
                    pause = true;
                    GameObject quantityHandler = Instantiate(Resources.Load<GameObject>("Inventory/Quantity Handler"), GameObject.Find("Canvas").transform, false);
                    quantityHandler.name = "Quantity Handler";
                    quantityHandler.transform.GetChild(5).GetComponent<Slider>().maxValue = Convert.ToInt32(src[slotNum - 1]["Quantity"]);
                    //quantityHandler.transform.GetChild(5).GetComponent<Slider>().value = quantityHandler.transform.GetChild(5).GetComponent<Slider>().maxValue;
                    quantityHandler.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Done(src, dst, slotNum));
                    IdentifyQuantity(quantityHandler.transform.GetChild(5).GetComponent<Slider>().value);
                }
                else
                {
                    Transfer(src, dst, slotNum - 1);
                }
                Inventory.UpdateInventory("Hotbar", db.hotbarItem);
                Inventory.UpdateInventory("Flask", db.flaskItem);
            }
        }
    }

    private void Done(Dictionary<string, object>[] src, Dictionary<string, object>[] dst, int slotNum)
    {
        float sliderValue = Mathf.Floor(GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().value);
        RepeatTransfer(src, dst, slotNum, sliderValue);
        pause = false;
        Inventory.UpdateInventory("Hotbar", db.hotbarItem);
        Inventory.UpdateInventory("Flask", db.flaskItem);
        Destroy(GameObject.Find("Quantity Handler"));
    }

    private bool RepeatTransfer(Dictionary<string, object>[] src, Dictionary<string, object>[] dst, int slotNum, float quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            if (!Transfer(src, dst, slotNum - 1))
            {
                return false;
            }
        }
        return true;
    }

    private bool Transfer(Dictionary<string, object>[] src, Dictionary<string, object>[] dst, int slotNum)
    {
        string[] allDstItemNames = new string[dst.Length];
        for (int i = 0; i < dst.Length; i++)
        {
            if (dst[i] != null)
            {
                allDstItemNames[i] = Convert.ToString(dst[i]["Item"]);
            }
        }
        if (allDstItemNames.Contains(Convert.ToString(src[slotNum]["Item"])))
        {
            for (int i = 0; i < dst.Length; i++)
            {
                if (Convert.ToString(src[slotNum]["Item"]) == allDstItemNames[i] && Convert.ToInt32(dst[i]["Quantity"]) < 64)
                {
                    dst[i]["Quantity"] = Convert.ToInt32(dst[i]["Quantity"]) + 1;
                    src[slotNum]["Quantity"] = Convert.ToInt32(src[slotNum]["Quantity"]) - 1;
                    return true;
                }
            }
            for (int i = 0; i < dst.Length; i++)
            {
                if (Convert.ToString(src[slotNum]["Item"]) == allDstItemNames[i] && Convert.ToInt32(dst[i]["Quantity"]) >= 64)
                {
                    if (ItemNotFound(src, dst, slotNum))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        else
        {
            if (ItemNotFound(src, dst, slotNum))
            {
                return true;
            }
        }
        return false;
    }

    private bool ItemNotFound(Dictionary<string, object>[] src, Dictionary<string, object>[] dst, int slotNum)
    {
        if (dst.Contains(null))
        {
            for (int i = 0; i < dst.Length; i++)
            {
                if (dst[i] == null)
                {
                    dst[i] = new Dictionary<string, object>()
                        {
                            {"Item", src[slotNum]["Item"]},
                            {"Quantity", 1}
                        };
                    src[slotNum]["Quantity"] = Convert.ToInt32(src[slotNum]["Quantity"]) - 1;
                    return true;
                }
            }
        }
        else
        {
            //Alert
            print("No more space");
        }
        return false;
    }
}
