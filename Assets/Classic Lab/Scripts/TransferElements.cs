using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransferElements : MonoBehaviour
{
    public void UpdateQuantity(float value)
    {
        GameObject.Find("Element Constructor Quantity Handler/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(Mathf.Floor(value));
        IdentifyQuantity(Mathf.Floor(value));
    }

    public void AddSliderValue()
    {
        if (GameObject.Find("Element Constructor Quantity Handler/Slider").GetComponent<Slider>().value + 1 <= GameObject.Find("Element Constructor Quantity Handler/Slider").GetComponent<Slider>().maxValue)
        {
            GameObject.Find("Element Constructor Quantity Handler/Slider").GetComponent<Slider>().value += 1;
        }
    }

    public void RemoveSliderValue()
    {
        if (GameObject.Find("Element Constructor Quantity Handler/Slider").GetComponent<Slider>().value - 1 >= GameObject.Find("Element Constructor Quantity Handler/Slider").GetComponent<Slider>().minValue)
        {
            GameObject.Find("Element Constructor Quantity Handler/Slider").GetComponent<Slider>().value -= 1;
        }
    }

    public void MaxSliderValue()
    {
        GameObject.Find("Element Constructor Quantity Handler/Slider").GetComponent<Slider>().value = GameObject.Find("Element Constructor Quantity Handler/Slider").GetComponent<Slider>().maxValue;
    }

    private void IdentifyQuantity(float quantity)
    {
        if (quantity == 0)
        {
            GameObject.Find("Element Constructor Quantity Handler/Done/Text").GetComponent<TextMeshProUGUI>().text = "Cancel";
        }
        else
        {
            GameObject.Find("Element Constructor Quantity Handler/Done/Text").GetComponent<TextMeshProUGUI>().text = "Done";
        }
    }

    public void TransferElementsToHotbar()
    {
        if (!QuantityHandler.pause)
        {
            if (ElementConstructor.constructedElement == null)
            {
                Alert.AddAlert("Nothing to construct in element constructor!");
                return;
            }
            QuantityHandler.pause = true;
            GameObject quantityHandler = Instantiate(Resources.Load<GameObject>("Inventory/Element Constructor Quantity Handler"), GameObject.Find("Canvas").transform, false);
            quantityHandler.name = "Element Constructor Quantity Handler";
            quantityHandler.transform.GetChild(5).GetComponent<Slider>().maxValue = 64;
            quantityHandler.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Done(Database.db.hotbarItem));
            quantityHandler.transform.GetChild(6).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ElementConstructor.constructedElement;
            IdentifyQuantity(quantityHandler.transform.GetChild(5).GetComponent<Slider>().value);
        }
    }

    private void Done(Dictionary<string, object>[] dst)
    {
        float sliderValue = Mathf.Floor(GameObject.Find("Element Constructor Quantity Handler/Slider").GetComponent<Slider>().value);
        RepeatTransfer(dst, sliderValue);
        QuantityHandler.pause = false;
        Global.UpdateInventory("Hotbar", dst);
        Destroy(GameObject.FindGameObjectWithTag("Quantity Handler"));
    }

    private bool RepeatTransfer(Dictionary<string, object>[] dst, float quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            if (!Transfer(dst))
            {
                return false;
            }
        }
        return true;
    }

    private bool Transfer(Dictionary<string, object>[] dst)
    {
        string[] allDstItemNames = new string[dst.Length];
        for (int i = 0; i < dst.Length; i++)
        {
            if (dst[i] != null)
            {
                allDstItemNames[i] = Convert.ToString(dst[i]["Item"]);
            }
        }
        if (allDstItemNames.Contains(ElementConstructor.constructedElement))
        {
            for (int i = 0; i < dst.Length; i++)
            {
                if (ElementConstructor.constructedElement == allDstItemNames[i] && Convert.ToInt32(dst[i]["Quantity"]) < 64)
                {
                    dst[i]["Quantity"] = Convert.ToInt32(dst[i]["Quantity"]) + 1;
                    return true;
                }
            }
            for (int i = 0; i < dst.Length; i++)
            {
                if (ElementConstructor.constructedElement == allDstItemNames[i] && Convert.ToInt32(dst[i]["Quantity"]) >= 64)
                {
                    if (ItemNotFound(dst))
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
            if (ItemNotFound(dst))
            {
                return true;
            }
        }
        return false;
    }

    private bool ItemNotFound(Dictionary<string, object>[] dst)
    {
        if (dst.Contains(null))
        {
            for (int i = 0; i < dst.Length; i++)
            {
                if (dst[i] == null)
                {
                    dst[i] = new Dictionary<string, object>()
                        {
                            {"Item", ElementConstructor.constructedElement},
                            {"Quantity", 1}
                        };
                    return true;
                }
            }
        }
        else
        {
            Alert.AddAlert("No enough space.");
        }
        return false;
    }
}
