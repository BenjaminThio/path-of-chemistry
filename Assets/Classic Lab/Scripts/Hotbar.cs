using System;
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
                if (db.hotbarItem[db.slotNum - 1] != null)
                {
                    ItemNameAppear(Convert.ToString(db.hotbarItem[db.slotNum - 1]["Item"]));
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
    public void ItemNameAppear(string itemName)
    {
        if (itemName != null)
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
