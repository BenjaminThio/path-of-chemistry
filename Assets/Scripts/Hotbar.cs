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
