using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Hotbar : MonoBehaviour
{
    private Database db;

    private void Start()
    {
        db = Database.db;
        for (int i = 1; i <= db.hotbarItem.Length; i++)
        {
            GameObject.Find($"Hotbar/Slot ({i})").GetComponent<Button>().onClick.AddListener(ToggleSlot);
        }
        UpdateSlot();
        Global.UpdateInventory("Hotbar", db.hotbarItem);
    }

    public void ToggleSlot()
    {
        if (!QuantityHandler.pause)
        {
            string selectedSlotName = EventSystem.current.currentSelectedGameObject.name;
            if (Global.Digitize(selectedSlotName) != db.slotNum)
            {
                db.slotNum = Global.Digitize(selectedSlotName);
                UpdateSlot();
                GameObject.FindGameObjectWithTag("Hand").GetComponent<Hand>().ChangeItemOnHand();
                if (db.hotbarItem[db.slotNum - 1] != null)
                {
                    ItemNameAppear(Convert.ToString(db.hotbarItem[db.slotNum - 1]["Item"]), true, false);
                }
            }
        }
    }

    public void TransferHotbarToFlask()
    {
        gameObject.GetComponent<QuantityHandler>().Estimation("Hotbar", db.hotbarItem, "Flask", db.flaskItem, db.slotNum);
    }

    public void TransferHotbarToCompoundCreator()
    {
        gameObject.GetComponent<QuantityHandler>().Estimation("Hotbar", db.hotbarItem, "Compound Creator", db.compoundCreatorItem, db.slotNum);
    }

    public void TransferHotbarToCompoundReducer()
    {
        string[] compounds = new string[Recipe.compounds.Count];
        for (int i = 0; i < Recipe.compounds.Count; i++)
        {
            compounds[i] = Recipe.compounds.ElementAt(i).Key.Key;
        }
        if (db.hotbarItem[db.slotNum - 1] != null)
        {
            if (Global.IsItemExist(Convert.ToString(db.hotbarItem[db.slotNum - 1]["Item"]), compounds))
            {
                gameObject.GetComponent<QuantityHandler>().Estimation("Hotbar", db.hotbarItem, "Original", db.compoundReducerOriginalItem, db.slotNum);
            } 
            else
            {
                Alert.AddAlert("Compound only.");
            }
        }
        else
        {
            Alert.AddAlert($"Nothing to add to \"Compound Reducer\".");
        }
    }

    public void UpdateSlot()
    {
        for (int i = 1; i <= Database.db.hotbarItem.Length; i++)
        {
            if (i == Database.db.slotNum)
            {
                GameObject.Find($"Hotbar/Slot ({i})").GetComponent<Image>().color = Color.cyan;
            }
            else
            {
                GameObject.Find($"Hotbar/Slot ({i})").GetComponent<Image>().color = Color.white;
            }
        }
    }
    public void ItemNameAppear(string itemName, bool isHotbar, bool handHandlerActive)
    {
        if (!isHotbar)
        {
            GameObject.FindGameObjectWithTag("Quantity Handler").transform.GetChild(6).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = itemName;
        }
        else if (itemName != null)
        {
            StopAllCoroutines();
            DeleteItemName();
            if (GameObject.FindGameObjectWithTag("Item Name") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("Item Name"));
            }
            GameObject itemNameContainer = Instantiate(Resources.Load<GameObject>("UI/Black Container"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
            itemNameContainer.GetComponentInChildren<TextMeshProUGUI>().text = itemName;
            StartCoroutine(WaitAndDeleteItemName(3f));
            if (handHandlerActive)
            {
                GameObject.FindGameObjectWithTag("Hand").GetComponent<Hand>().ChangeItemOnHand();
            }
        }
    }

    private IEnumerator WaitAndDeleteItemName(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        DeleteItemName();
    }

    private void DeleteItemName()
    {
        if (GameObject.FindGameObjectWithTag("Item Name") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Item Name"));
        }
    }
}
