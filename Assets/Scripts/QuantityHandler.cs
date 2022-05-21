using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuantityHandler : MonoBehaviour
{
    public Database db;
    public static bool pause = false;
    public static int selectedSlotNum;

    private void Awake()
    {
        db = Database.Load();
        UpdateInventory("Flask Interface/Flask", db.flaskItem);
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
                {"Hotbar", db.hotbarItem},
                {"Flask", db.flaskItem},
            },
            Hotbar.slotNum
        );
    }

    public void TransferFlaskToHotbar()
    {
        if (!pause)
        {
            selectedSlotNum = Hotbar.Digitize(EventSystem.current.currentSelectedGameObject.name);
            if (db.flaskItem[selectedSlotNum - 1] != null)
            {
                GameObject.Find($"Flask/Slot ({selectedSlotNum})").GetComponent<Image>().color = Color.cyan;
            }
        }
        Estimation(new Dictionary<string, Dictionary<string, object>[]>(){
                {"Flask", db.flaskItem},
                {"Hotbar", db.hotbarItem},
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
                    for (int i = 1; i <= db.flaskItem.Length; i++)
                    {
                        GameObject.Find($"Flask Interface/Flask/Slot ({i})").GetComponent<Image>().color = Color.grey;
                    }
                }
                UpdateInventory("Hotbar", db.hotbarItem);
                UpdateInventory("Flask Interface/Flask", db.flaskItem);
            }
        }
    }

    private void Done(Dictionary<string, Dictionary<string, object>[]> data, int slotNum)
    {
        float sliderValue = Mathf.Floor(GameObject.Find("Quantity Handler/Slider").GetComponent<Slider>().value);
        Dictionary<string, Dictionary<string, object>[]> newData = RepeatTransfer(data, slotNum, sliderValue);
        pause = false;
        for (int i = 1; i <= db.flaskItem.Length; i++)
        {
            GameObject.Find($"Flask Interface/Flask/Slot ({i})").GetComponent<Image>().color = Color.grey;
        }
        if (newData != null)
        {
            db.hotbarItem = newData["Hotbar"];
            db.flaskItem = newData["Flask"];
        }
        UpdateInventory("Hotbar", db.hotbarItem);
        UpdateInventory("Flask Interface/Flask", db.flaskItem);
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

    public static void UpdateInventory(string path, Dictionary<string, object>[] inventory)
    {
        for (int i = 1; i <= inventory.Length; i++)
        {
            GameObject.Find($"{path}/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
            if (GameObject.Find($"{path}/Slot ({i})/Item") == null)
            {
                GameObject item = Instantiate(Resources.Load<GameObject>("Inventory/Item"), GameObject.Find($"{path}/Slot ({i})").transform);
                item.name = "Item";
                item.transform.SetAsFirstSibling();
            }
            if (inventory[i - 1] != null)
            {
                if (Convert.ToInt32(inventory[i - 1]["Quantity"]) == 0)
                {
                    inventory[i - 1] = null;
                    Destroy(GameObject.Find($"{path}/Slot ({i})/Item"));
                    continue;
                }
                else if (Convert.ToInt32(inventory[i - 1]["Quantity"]) > 1)
                {
                    GameObject.Find($"{path}/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = Convert.ToString(inventory[i - 1]["Quantity"]);
                }
                else
                {
                    GameObject.Find($"{path}/Slot ({i})/Quantity").GetComponent<TextMeshProUGUI>().text = "";
                }
                GameObject.Find($"{path}/Slot ({i})/Item").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Elements/{inventory[i - 1]["Item"]}");
            }
            else
            {
                Destroy(GameObject.Find($"{path}/Slot ({i})/Item"));
            }
        }
    }
}
