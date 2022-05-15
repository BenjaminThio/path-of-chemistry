using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuantityHandler : MonoBehaviour
{
    public static int selectedSlotNum;
    public static Dictionary<string, object>[] flaskItem = {
        null,
        null,
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        null,
        null,
        null,
        null
    };

    private void Awake()
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

    public void TransferHotbarToFlask()
    {
        Estimation("Hotbar", Hotbar.hotbarItem, "Flask", flaskItem, Hotbar.slotNum);
    }

    public void TransferFlaskToHotbar()
    {
        selectedSlotNum = Digitize(EventSystem.current.currentSelectedGameObject.name);
        Estimation("Flask", flaskItem, "Hotbar", Hotbar.hotbarItem, selectedSlotNum);
    }

    private void Estimation(string srcName, Dictionary<string, object>[] src, string dstName, Dictionary<string, object>[] dst, int slotNum)
    {
        if (src[slotNum - 1] != null)
        {
            if (Convert.ToInt32(src[slotNum - 1]["Quantity"]) > 1)
            {
                GameObject quantityHandler = Instantiate(Resources.Load<GameObject>("Inventory/Quantity Handler"), GameObject.Find("Canvas").transform, false);
                quantityHandler.name = "Quantity Handler";
                quantityHandler.transform.GetChild(5).GetComponent<Slider>().maxValue = Convert.ToInt32(src[slotNum - 1]["Quantity"]);
                quantityHandler.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Done(
                    new Dictionary<string, Dictionary<string, object>[]>()
                    {
                        {srcName, src},
                        {dstName, dst}
                    },
                    slotNum)
                );
            }
            else
            {
                Transfer(src, dst, slotNum - 1);
            }
            UpdateFlask();
            UpdateHotbar();
        }
    }

    private void Done(Dictionary<string, Dictionary<string, object>[]> data, int slotNum)
    {
        int sliderValue = Convert.ToInt32(Mathf.Floor(GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().value));
        var newData = RepeatTransfer(data, slotNum, sliderValue);
        Hotbar.hotbarItem = newData["Hotbar"];
        flaskItem = newData["Flask"];
        UpdateFlask();
        UpdateHotbar();
        Destroy(GameObject.Find("Quantity Handler"));
    }

    private Dictionary<string, Dictionary<string, object>[]> RepeatTransfer(Dictionary<string, Dictionary<string, object>[]> data, int slotNum, int quantity)
    {
        var src = data.ElementAt(0).Value;
        var dst = data.ElementAt(1).Value;
        for (int i = 0; i < quantity; i++)
        {
            var newData = Transfer(src, dst, slotNum - 1);
            src = newData[0];
            dst = newData[1];
        }
        return new Dictionary<string, Dictionary<string, object>[]>()
        {
            {data.ElementAt(0).Key, src},
            {data.ElementAt(1).Key, dst}
        };
    }

    private List<Dictionary<string, object>[]> Transfer(Dictionary<string, object>[] src, Dictionary<string, object>[] dst, int slotNum)
    {
        List<string> allDstItemNames = new List<string>();
        foreach (var i in dst)
        {
            if (i != null)
            {
                allDstItemNames.Add(Convert.ToString(i["Item"]));
            }
            else
            {
                allDstItemNames.Add(null);
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
                    return new List<Dictionary<string, object>[]>
                    {
                        src,
                        dst
                    };
                }
            }
            for (int i = 0; i < dst.Length; i++)
            {
                if (Convert.ToString(src[slotNum]["Item"]) == allDstItemNames[i] && Convert.ToInt32(dst[i]["Quantity"]) >= 64)
                {
                    var newData = ItemNotFound(src, dst, slotNum);
                    return new List<Dictionary<string, object>[]>
                    {
                        newData[0],
                        newData[1]
                    };
                }
            }
        }
        else
        {
            var newData = ItemNotFound(src, dst, slotNum);
            return new List<Dictionary<string, object>[]>
            {
                newData[0],
                newData[1]
            };
        }
        return null;
    }

    private List<Dictionary<string, object>[]> ItemNotFound(Dictionary<string, object>[] src, Dictionary<string, object>[] dst, int slotNum)
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
                    return new List<Dictionary<string, object>[]>
                    {
                        src,
                        dst
                    };
                }
            }
        }
        else
        {
            //Alert
            print("No more space");
        }
        return null;
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

    private void UpdateFlask()
    {
        for (int i = 1; i <= flaskItem.Length; i++)
        {
            GameObject.Find($"Flask Interface/Flask/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
            if (GameObject.Find($"Flask Interface/Flask/Slot ({i})/Item") == null)
            {
                GameObject item = Instantiate(Resources.Load<GameObject>("Inventory/Item"), GameObject.Find($"Flask Interface/Flask/Slot ({i})").transform);
                item.name = "Item";
                item.transform.SetAsFirstSibling();
            }
            if (flaskItem[i - 1] != null)
            {
                if (Convert.ToInt32(flaskItem[i - 1]["Quantity"]) == 0)
                {
                    flaskItem[i - 1] = null;
                    Destroy(GameObject.Find($"Flask Interface/Flask/Slot ({i})/Item"));
                    continue;
                }
                else if (Convert.ToInt32(flaskItem[i - 1]["Quantity"]) > 1)
                {
                    GameObject.Find($"Flask Interface/Flask/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(flaskItem[i - 1]["Quantity"]);
                }
                else
                {
                    GameObject.Find($"Flask Interface/Flask/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
                }
                GameObject.Find($"Flask Interface/Flask/Slot ({i})/Item").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Elements/{flaskItem[i - 1]["Item"]}");
            }
            else
            {
                Destroy(GameObject.Find($"Flask Interface/Flask/Slot ({i})/Item"));
            }
        }
    }

    private void UpdateHotbar()
    {
        for (int i = 1; i <= Hotbar.hotbarItem.Length; i++)
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
