using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CompoundReducerReduced : MonoBehaviour
{
    private Database db;
    private static int selectedSlotNum;

    private void Start()
    {
        db = Database.db;
        for (int i = 1; i <= db.compoundReducerReducedItem.Length; i++)
        {
            GameObject.Find($"Reduced/Slot ({i})").GetComponent<Button>().onClick.AddListener(TransferReducedToHotbar);
        }
        Global.UpdateInventory("Reduced", db.compoundReducerReducedItem);
    }

    public void TransferReducedToHotbar()
    {
        if (!QuantityHandler.pause)
        {
            selectedSlotNum = Global.Digitize(EventSystem.current.currentSelectedGameObject.name);
            if (db.compoundReducerReducedItem[selectedSlotNum - 1] != null)
            {
                GameObject.Find($"Reduced/Slot ({selectedSlotNum})").GetComponent<Image>().color = Color.cyan;
            }
        }
        gameObject.GetComponent<QuantityHandler>().Estimation("Reduced", db.compoundReducerReducedItem, "Hotbar", db.hotbarItem, selectedSlotNum);
    }
}
