using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Flask : MonoBehaviour
{
    private Database db;
    private static int selectedSlotNum;

    private void Start()
    {
        db = Database.db;
        for (int i = 1; i <= db.flaskItem.Length; i++)
        {
            GameObject.Find($"Flask/Slot ({i})").GetComponent<Button>().onClick.AddListener(TransferFlaskToHotbar);
        }
        Global.UpdateInventory("Flask", db.flaskItem);
    }

    public void TransferFlaskToHotbar()
    {
        if (!QuantityHandler.pause)
        {
            selectedSlotNum = Global.Digitize(EventSystem.current.currentSelectedGameObject.name);
            if (db.flaskItem[selectedSlotNum - 1] != null)
            {
                GameObject.Find($"Flask/Slot ({selectedSlotNum})").GetComponent<Image>().color = Color.cyan;
            }
        }
        gameObject.GetComponent<QuantityHandler>().Estimation("Flask", db.flaskItem, "Hotbar", db.hotbarItem, selectedSlotNum);
    }
}
