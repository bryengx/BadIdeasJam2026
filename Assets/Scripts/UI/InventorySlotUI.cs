using UnityEngine;
using UnityEngine.UI;
public class InventorySlotUI : MonoBehaviour
{
    public Image icon;

    public void SetSlot(InventorySlot slot)
    {
        if (slot == null || slot.item == null)
        {
            icon.enabled = false;
            return;
        }

        icon.enabled = true;
        icon.sprite = slot.item.icon;
    }

    public void Clear()
    {
        icon.enabled = false;
    }
}