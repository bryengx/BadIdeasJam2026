using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();

    public void AddItem(InventoryItem item, int amount = 1)
    {
        if (item == null)
            return;

        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (slot.item == item)
                {
                    slot.Add(amount);
                    return;
                }
            }
        }

        InventorySlot newSlot = new InventorySlot(item, amount);
        slots.Add(newSlot);
    }

    public void RemoveItem(InventoryItem item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                slot.Remove(amount);

                if (slot.IsEmpty())
                    slots.Remove(slot);

                return;
            }
        }
    }

    public int GetItemCount(InventoryItem item)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item)
                return slot.quantity;
        }

        return 0;
    }
}