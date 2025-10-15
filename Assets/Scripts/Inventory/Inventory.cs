using System;
using System.Collections.Generic;
using System.Linq;
using Generals;
using UnityEngine;

public class ItemStack
{
    public ItemStack(ItemSO itemType, int amount)
    {
        ItemType = itemType;
        Amount = amount;
    }

    public ItemSO ItemType { get; set; }
    public int Amount { get; set; }
}

public class Inventory : Singleton<Inventory>
{
    public int maxInventorySize = 20;
    public int maxStackSize = 99;
    public Dictionary<int, ItemStack> slotToStack = new();

    public event Action<ItemSO, int> OnItemAdded;
    public event Action<ItemSO, int> OnItemRemoved;
    public event Action<int, int> OnItemMoved;
    public event Action<int, int> OnItemSwapped;

    public bool AddItem(ItemSO itemType)
    {
        if (slotToStack.Count >= maxInventorySize)
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        var slot = FindStackableSlot(itemType);

        if (slot != -1)
        {
            slotToStack[slot].Amount += 1;
        }
        else
        {
            var emptySlot = GetFirstEmptySlot();

            if (emptySlot >= maxInventorySize || emptySlot == -1) return false;

            var newStack = new ItemStack(itemType, 1);
            slotToStack.Add(emptySlot, newStack);
            slot = emptySlot;
        }

        if (OnItemAdded != null)
        {
            OnItemAdded.Invoke(itemType, slot);
        }
        Debug.Log($"Added {itemType.name} to inventory at slot {slot}. Stack amount: {slotToStack[slot].Amount}");
        return true;
    }

    private int FindStackableSlot(ItemSO itemType)
    {
        foreach (var kvp in slotToStack)
            if (kvp.Value.ItemType == itemType && kvp.Value.Amount < maxStackSize)
                return kvp.Key;

        return -1;
    }

    private int GetFirstEmptySlot()
    {
        for (var i = 0; i < maxInventorySize; i++)
            if (!slotToStack.ContainsKey(i))
                return i;

        return -1;
    }

    public bool RemoveItem(ItemSO itemType)
    {
        var slot = FindSlotWithItem(itemType);

        if (slot != -1)
        {
            slotToStack[slot].Amount -= 1;

            if (slotToStack[slot].Amount <= 0) slotToStack.Remove(slot);
            if (OnItemRemoved != null)
            {
                OnItemRemoved.Invoke(itemType, slot);
            }
            Debug.Log($"Removed {itemType.name} from inventory. Items remaining: {slotToStack.Count}");
            return true;
        }

        Debug.Log($"{itemType.name} not found in inventory.");
        return false;
    }

    private int FindSlotWithItem(ItemSO itemType)
    {
        foreach (var kvp in slotToStack)
            if (kvp.Value.ItemType == itemType)
                return kvp.Key;

        return -1;
    }

    public bool TryMoveItem(int sourceSlot, int targetSlot)
    {
        Debug.Log("TryMoveItem");

        if (!slotToStack.ContainsKey(sourceSlot))
        {
            Debug.LogError($"Source slot {sourceSlot} does not contain an item!");
            return false;
        }

        if (!slotToStack.ContainsKey(targetSlot))
        {
            var stack = slotToStack[sourceSlot];
            slotToStack.Add(targetSlot, stack);
            slotToStack.Remove(sourceSlot);

            if (OnItemMoved != null)
            {
                OnItemMoved.Invoke(sourceSlot, targetSlot);
            }
        }
        else
        {
            if (!slotToStack.SwapEntries(sourceSlot, targetSlot))
            {
                Debug.LogError("Failed to swap slotToStack dictionary entries");
                return false;
            }
            if (OnItemSwapped != null)
            {
                OnItemSwapped.Invoke(sourceSlot, targetSlot);
            }
        }

        return true;
    }

    public List<ItemStack> GetStacks()
    {
        return slotToStack.Values.ToList();
    }


    public bool HasItem(ItemSO itemType)
    {
        return FindSlotWithItem(itemType) != -1;
    }

    public int GetItemCount()
    {
        return slotToStack.Count;
    }

    public void ClearInventory()
    {
        slotToStack.Clear();
        Debug.Log("Inventory cleared.");
    }
}