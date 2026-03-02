using UnityEngine;
using System;

[Serializable]
public class InventorySlot
{
    public InventoryItem item;
    public int quantity;

    public InventorySlot(InventoryItem item, int amount)
    {
        this.item = item;
        this.quantity = amount;
    }

    public void Add(int amount)
    {
        quantity += amount;
    }

    public void Remove(int amount)
    {
        quantity -= amount;
        if (quantity < 0)
            quantity = 0;
    }

    public bool IsEmpty()
    {
        return item == null || quantity <= 0;
    }
}