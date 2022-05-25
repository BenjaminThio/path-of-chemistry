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
