using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    private Database db;

    private void Start()
    {
        db = Database.db;
    }

    public static int GetSlotNum(string Text)
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

    public static void UpdateInventory(string path, Dictionary<string, object>[] inventory)
    {
        for (int i = 1; i <= inventory.Length; i++)
        {
            GameObject.Find($"{path}/Slot ({i})").GetComponent<Image>().color = Color.grey;
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
