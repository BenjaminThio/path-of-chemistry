using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuantityHandler : MonoBehaviour
{
    public static bool pause = false;
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

    public void TransferHotbarToFlask()
    {
        Estimation(new Dictionary<string, Dictionary<string, object>[]>(){
                {"Hotbar", Hotbar.hotbarItem},
                {"Flask", flaskItem},
            },
            Hotbar.slotNum
        );
    }

    public void TransferFlaskToHotbar()
    {
        if (!pause)
        {
            selectedSlotNum = Digitize(EventSystem.current.currentSelectedGameObject.name);
            if (flaskItem[selectedSlotNum - 1] != null)
            {
                GameObject.Find($"Flask/Slot ({selectedSlotNum})").GetComponent<Image>().color = Color.cyan;
            }
        }
        Estimation(new Dictionary<string, Dictionary<string, object>[]>(){
                {"Flask", flaskItem},
                {"Hotbar", Hotbar.hotbarItem},
            },
            selectedSlotNum
        );
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

    private void Estimation(Dictionary<string, Dictionary<string, object>[]> data, int slotNum)
    {
        string srcName = data.ElementAt(0).Key;
        Dictionary<string, object>[] src = data.ElementAt(0).Value;
        string dstName = data.ElementAt(1).Key;
        Dictionary<string, object>[] dst = data.ElementAt(1).Value;
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
                    quantityHandler.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Done(
                        new Dictionary<string, Dictionary<string, object>[]>()
                        {
                        {srcName, src},
                        {dstName, dst}
                        },
                        slotNum)
                    );
                    IdentifyQuantity(quantityHandler.transform.GetChild(5).GetComponent<Slider>().value);
                }
                else
                {
                    Transfer(src, dst, slotNum - 1);
                    for (int i = 1; i <= flaskItem.Length; i++)
                    {
                        GameObject.Find($"Flask Interface/Flask/Slot ({i})").GetComponent<Image>().color = Color.grey;
                    }
                }
                UpdateFlask();
                UpdateHotbar();
            }
        }
    }

    private void Done(Dictionary<string, Dictionary<string, object>[]> data, int slotNum)
    {
        float sliderValue = Mathf.Floor(GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().value);
        Dictionary<string, Dictionary<string, object>[]> newData = RepeatTransfer(data, slotNum, sliderValue);
        pause = false;
        for (int i = 1; i <= flaskItem.Length; i++)
        {
            GameObject.Find($"Flask Interface/Flask/Slot ({i})").GetComponent<Image>().color = Color.grey;
        }
        if (newData != null)
        {
            Hotbar.hotbarItem = newData["Hotbar"];
            flaskItem = newData["Flask"];
        }
        UpdateHotbar();
        UpdateFlask();
        Destroy(GameObject.Find("Quantity Handler"));
    }

    private Dictionary<string, Dictionary<string, object>[]> RepeatTransfer(Dictionary<string, Dictionary<string, object>[]> data, int slotNum, float quantity)
    {
        string srcName = data.ElementAt(0).Key;
        Dictionary<string, object>[] src = data.ElementAt(0).Value;
        string dstName = data.ElementAt(1).Key;
        Dictionary<string, object>[] dst = data.ElementAt(1).Value;
        for (int i = 0; i < quantity; i++)
        {
            Dictionary<string, object>[][] newData = Transfer(src, dst, slotNum - 1);
            if (newData != null)
            {
                src = newData[0];
                dst = newData[1];
            }
            else
            {
                return null;
            }
        }
        return new Dictionary<string, Dictionary<string, object>[]>()
        {
            {srcName, src},
            {dstName, dst}
        };
    }

    private Dictionary<string, object>[][] Transfer(Dictionary<string, object>[] src, Dictionary<string, object>[] dst, int slotNum)
    {
        /*List<string> allDstItemNames = new List<string>();
        foreach (Dictionary<string, object> i in dst)
        {
            if (i != null)
            {
                allDstItemNames.Add(Convert.ToString(i["Item"]));
            }
            else
            {
                allDstItemNames.Add(null);
            }
        }*/
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
                    return new Dictionary<string, object>[][]
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
                    Dictionary<string, object>[][] newData = ItemNotFound(src, dst, slotNum);
                    if (newData != null)
                    {
                        return new Dictionary<string, object>[][]
                        {
                            newData[0],
                            newData[1]
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        else
        {
            Dictionary<string, object>[][] newData = ItemNotFound(src, dst, slotNum);
            if (newData != null)
            {
                return new Dictionary<string, object>[][]
                {
                    newData[0],
                    newData[1]
                };
            }
        }
        return null;
    }

    private Dictionary<string, object>[][] ItemNotFound(Dictionary<string, object>[] src, Dictionary<string, object>[] dst, int slotNum)
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
                    return new Dictionary<string, object>[][]
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
