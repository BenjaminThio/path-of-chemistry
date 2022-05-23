using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Hotbar : MonoBehaviour
{
    private Database db;

    private void Start()
    {
        db = Database.db;
        UpdateSlot();
        Inventory.UpdateInventory("Hotbar", db.hotbarItem);
    }

    public void ToggleSlot()
    {
        if (!QuantityHandler.pause)
        {
            var selectedSlotName = EventSystem.current.currentSelectedGameObject.name;
            if (Inventory.GetSlotNum(selectedSlotName) != db.slotNum)
            {
                db.slotNum = Inventory.GetSlotNum(selectedSlotName);
                UpdateSlot();
            }
        }
    }

    private void UpdateSlot()
    {
        for (int i = 1; i <= db.hotbarItem.Length; i++)
        {
            if (i == db.slotNum)
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
