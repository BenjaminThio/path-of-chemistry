using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Global : MonoBehaviour
{
    private Database db;

    private void Start()
    {
        db = Database.db;
    }

    public static bool isExist(string originalTag, string[] allowTags)
    {
        foreach (string tag in allowTags)
        {
            if (originalTag == tag)
            {
                return true;
            }
        }
        return false;
    }

    public static int Digitize(string text)
    {
        text = text.Trim();
        char[] charArr = text.ToCharArray();
        List<char> digitList = new List<char>();
        foreach (char i in charArr)
        {
            if (char.IsDigit(i))
            {
                digitList.Add(i);
            }
        }
        if (digitList.Count == 0)
        {
            return 0;
        }
        return Convert.ToInt32(new string(digitList.ToArray()));
    }

    public static bool IsDigit(string text)
    {
        text = text.Trim();
        if (text == "")
        {
            return false;
        }
        char[] charArr = text.ToCharArray();
        foreach (char i in charArr)
        {
            if (!char.IsDigit(i))
            {
                return false;
            }
        }
        return true;
    }

    public static Dictionary<string, int> CreateVirtualProps(Dictionary<string, object>[] inventoryItems)
    {
        Dictionary<string, int> props = new Dictionary<string, int>();
        foreach (Dictionary<string, object> prop in inventoryItems)
        {
            if (prop != null)
            {
                if (!props.ContainsKey(Convert.ToString(prop["Item"])))
                {
                    props.Add(Convert.ToString(prop["Item"]), Convert.ToInt32(prop["Quantity"]));
                }
                else
                {
                    props[Convert.ToString(prop["Item"])] += Convert.ToInt32(prop["Quantity"]);
                }
            }
        }
        return props;
    }

    public static bool ContainsProps(Dictionary<string, int> recipe, Dictionary<string, int> props)
    {
        foreach (KeyValuePair<string, int> material in recipe)
        {
            if (!(props.ContainsKey(material.Key) && props[material.Key].Equals(material.Value)))
            {
                return false;
            }
        }
        return true;
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
