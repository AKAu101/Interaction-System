using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public InventorySlot()
    {
        item = null;
        quantity = 0;
    }

    public InventorySlot(Item newItem, int amount)
    {
        item = newItem;
        quantity = amount;
    }

    public bool IsEmpty()
    {
        return item == null || quantity <= 0;
    }

    public bool AddItem(Item newItem, int amount)
    {
        if (item == null)
        {
            item = newItem;
            quantity = amount;
            return true;
        }

        if (item == newItem)
        {
            if (quantity + amount <= item.maxStackSize)
            {
                quantity += amount;
                return true;
            }
            else
            {
                int remainingSpace = item.maxStackSize - quantity;
                if (remainingSpace > 0)
                {
                    quantity = item.maxStackSize;
                    return false; // Partial add, stack full
                }
                return false; // Stack already full
            }
        }

        return false; // Different item
    }

    public bool RemoveItem(int amount)
    {
        if (quantity >= amount)
        {
            quantity -= amount;
            if (quantity <= 0)
            {
                Clear();
            }
            return true;
        }
        return false;
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}
