using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public int maxInventorySize = 20;

   public bool AddItem(GameObject item)
    {
        if (items.Count >= maxInventorySize)
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        items.Add(item);
        Debug.Log($"Added {item.name} to inventory. Items in inventory: {items.Count}");
        return true;
    }

    public bool RemoveItem(GameObject item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"Removed {item.name} from inventory. Items remaining: {items.Count}");
            return true;
        }

        Debug.Log($"{item.name} not found in inventory.");
        return false;
    }

    public bool HasItem(GameObject item)
    {
        return items.Contains(item);
    }

    public int GetItemCount()
    {
        return items.Count;
    }

    public void ClearInventory()
    {
        items.Clear();
        Debug.Log("Inventory cleared.");
    }
}
