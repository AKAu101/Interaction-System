using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnInventoryChangedEvent();
    public event OnInventoryChangedEvent OnInventoryChanged;

    public int maxSlots = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();

    void Awake()
    {
        slots = new List<InventorySlot>(maxSlots);
        for (int i = 0; i < maxSlots; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(Item item, int quantity = 1)
    {
        if (item == null || quantity <= 0)
        {
            Debug.LogWarning("Cannot add null item or invalid quantity.");
            return false;
        }

        int remainingQuantity = quantity;

        // Try to stack with existing items
        if (item.maxStackSize > 1)
        {
            foreach (InventorySlot slot in slots)
            {
                if (!slot.IsEmpty() && slot.item == item && slot.quantity < item.maxStackSize)
                {
                    int spaceInSlot = item.maxStackSize - slot.quantity;
                    int amountToAdd = Mathf.Min(spaceInSlot, remainingQuantity);
                    slot.quantity += amountToAdd;
                    remainingQuantity -= amountToAdd;

                    if (remainingQuantity <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        Debug.Log($"Added {quantity}x {item.itemName} to inventory (stacked).");
                        return true;
                    }
                }
            }
        }

        // Find empty slots for remaining quantity
        while (remainingQuantity > 0)
        {
            InventorySlot emptySlot = FindEmptySlot();
            if (emptySlot == null)
            {
                Debug.LogWarning($"Inventory full! Could not add all {item.itemName}.");
                if (remainingQuantity < quantity)
                {
                    OnInventoryChanged?.Invoke();
                }
                return false;
            }

            int amountToAdd = Mathf.Min(item.maxStackSize, remainingQuantity);
            emptySlot.item = item;
            emptySlot.quantity = amountToAdd;
            remainingQuantity -= amountToAdd;
        }

        OnInventoryChanged?.Invoke();
        Debug.Log($"Added {quantity}x {item.itemName} to inventory.");
        return true;
    }

    public bool RemoveItem(Item item, int quantity = 1)
    {
        if (item == null || quantity <= 0)
        {
            return false;
        }

        int remainingToRemove = quantity;

        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].IsEmpty() && slots[i].item == item)
            {
                if (slots[i].quantity >= remainingToRemove)
                {
                    slots[i].RemoveItem(remainingToRemove);
                    OnInventoryChanged?.Invoke();
                    Debug.Log($"Removed {quantity}x {item.itemName} from inventory.");
                    return true;
                }
                else
                {
                    remainingToRemove -= slots[i].quantity;
                    slots[i].Clear();
                }
            }
        }

        if (remainingToRemove < quantity)
        {
            OnInventoryChanged?.Invoke();
            Debug.LogWarning($"Could only remove partial amount of {item.itemName}.");
        }

        return false;
    }

    public InventorySlot GetSlot(int index)
    {
        if (index >= 0 && index < slots.Count)
        {
            return slots[index];
        }
        return null;
    }

    public void SwapSlots(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= slots.Count || indexB < 0 || indexB >= slots.Count)
        {
            Debug.LogWarning("Invalid slot indices for swapping.");
            return;
        }

        InventorySlot temp = slots[indexA];
        slots[indexA] = slots[indexB];
        slots[indexB] = temp;

        OnInventoryChanged?.Invoke();
    }

    public bool HasItem(Item item, int quantity = 1)
    {
        return GetItemCount(item) >= quantity;
    }

    public int GetItemCount(Item item)
    {
        if (item == null)
        {
            return 0;
        }

        int totalCount = 0;
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty() && slot.item == item)
            {
                totalCount += slot.quantity;
            }
        }
        return totalCount;
    }

    public void ClearInventory()
    {
        foreach (InventorySlot slot in slots)
        {
            slot.Clear();
        }
        OnInventoryChanged?.Invoke();
        Debug.Log("Inventory cleared.");
    }

    private InventorySlot FindEmptySlot()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                return slot;
            }
        }
        return null;
    }
}
