using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CompoundCreator : MonoBehaviour
{
    private Database db;
    private static int selectedSlotNum;

    private void Start()
    {
        db = Database.db;
        for (int i = 1; i <= db.compoundCreatorItem.Length; i++)
        {
            GameObject.Find($"Compound Creator/Slot ({i})").GetComponent<Button>().onClick.AddListener(TransferCompoundCreatorToHotbar);
        }
        Global.UpdateInventory("Compound Creator", db.compoundCreatorItem);
    }

    public void TransferCompoundCreatorToHotbar()
    {
        if (!QuantityHandler.pause)
        {
            selectedSlotNum = Global.Digitize(EventSystem.current.currentSelectedGameObject.name);
            if (db.compoundCreatorItem[selectedSlotNum - 1] != null)
            {
                GameObject.Find($"Compound Creator/Slot ({selectedSlotNum})").GetComponent<Image>().color = Color.cyan;
            }
        }
        gameObject.GetComponent<QuantityHandler>().Estimation("Compound Creator", db.compoundCreatorItem, "Hotbar", db.hotbarItem, selectedSlotNum);
    }
}
