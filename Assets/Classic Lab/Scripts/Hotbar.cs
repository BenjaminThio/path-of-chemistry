using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    public static void UpdateSlot()
    {
        for (int i = 1; i <= Database.db.hotbarItem.Length; i++)
        {
            if (i == Database.db.slotNum)
            {
                GameObject.Find($"Hotbar/Slot ({i})").GetComponent<Image>().color = Color.cyan;
            }
            else
            {
                GameObject.Find($"Hotbar/Slot ({i})").GetComponent<Image>().color = Color.grey;
            }
        }
    }
}
