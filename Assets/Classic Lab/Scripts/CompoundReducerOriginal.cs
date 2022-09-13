using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CompoundReducerOriginal : MonoBehaviour
{
    private Database db;
    private static int selectedSlotNum;

    private void Start()
    {
        db = Database.db;
        for (int i = 1; i <= db.compoundReducerOriginalItem.Length; i++)
        {
            GameObject.Find($"Original/Slot ({i})").GetComponent<Button>().onClick.AddListener(TransferOriginalToHotbar);
        }
        Global.UpdateInventory("Original", db.compoundReducerOriginalItem);
    }

    public void TransferOriginalToHotbar()
    {
        if (!QuantityHandler.pause)
        {
            selectedSlotNum = Global.Digitize(EventSystem.current.currentSelectedGameObject.name);
        }
        gameObject.GetComponent<QuantityHandler>().Estimation("Original", db.compoundReducerOriginalItem, "Hotbar", db.hotbarItem, selectedSlotNum);
    }
}
