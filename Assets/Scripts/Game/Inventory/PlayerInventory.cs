using System;
using System.Collections.Generic;
using UnityEngine;

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
                    InventoryUI.instance.UpdateUI(slots);
                    return;
                }
            }
        }

        InventorySlot newSlot = new InventorySlot(item, amount);
        slots.Add(newSlot);
        InventoryUI.instance.UpdateUI(slots);
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

                InventoryUI.instance.UpdateUI(slots);
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

    internal bool HasItem(InventoryItem keyItem)
    {
        foreach (var slot in slots)
        {
            if (slot.item == keyItem)
                return true;
        }

        return false;
    }
}