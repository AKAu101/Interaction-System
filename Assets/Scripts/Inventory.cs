using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Generals;

public struct ItemStack
{
    public ItemSO ItemType;
    public int Amount;
}

public class Inventory : Singleton<Inventory>
{
    //public List<Item> items;
    public Dictionary<int, ItemStack> ItemDictionary = new Dictionary<int, ItemStack>();
    public Dictionary<ItemSO, int> SlotDictionary = new Dictionary<ItemSO, int>();
    public int maxInventorySize = 20;
    [SerializeField] private GameObject ItemViewPrefab;

   public bool AddItem(ItemSO itemType)
    {
        if (ItemDictionary.Count >= maxInventorySize)
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        if(SlotDictionary.ContainsKey(itemType))
        {
            //get slot of item type
            int slot = SlotDictionary[itemType];
            var stack = ItemDictionary[slot];
            stack.Amount += 1;
            ItemDictionary[slot] = stack;

        }
        else
        {
            //get first empty
            int emptySlot = GetFirstEmptySlot();
            
            if (emptySlot >= maxInventorySize || emptySlot == -1) return false;
            
            
            //update dicts
            SlotDictionary.Add(itemType, emptySlot);
            ItemStack newStack = new ItemStack();
            newStack.Amount = 1;
            newStack.ItemType = itemType;
            ItemDictionary.Add(emptySlot,newStack);
            
            
            //create new itemview   
            var inst = Instantiate(ItemViewPrefab);
            ItemView view = inst.GetComponent<ItemView>();
            if(view == null)Debug.LogWarning("Bastard");
            
            //setup itemview
            view.Setup(itemType,emptySlot);
            //integrate
            InventoryGUI.Instance.IntegrateView(view);
            
        }
        //items.Add(item);
        //ItemDictionary.ContainsValue(Ite)
        Debug.Log($"Added {itemType.name} to inventory. Items of this type in inventory: {ItemDictionary[SlotDictionary[itemType]].Amount}]]");
        return true;
    }

    private int GetFirstEmptySlot()
    {
        for (int i = 0; i < maxInventorySize; i++)
        {
            if (!SlotDictionary.ContainsValue(i))
            {
                return i; 
            }
        }

        return -1;
    }

    public bool RemoveItem(ItemSO itemType)
    {
        if (SlotDictionary.ContainsKey(itemType))
        {
            int slot = SlotDictionary[itemType];
            ItemStack stack = ItemDictionary[slot];
            stack.Amount -= 1;
            if (stack.Amount >= 0)
            {
                //remove from slot dict
                SlotDictionary.Remove(itemType);
                //desotry view
            }
            //items.Remove(item);
            Debug.Log($"Removed {itemType.name} from inventory. Items remaining: {ItemDictionary.Count}");
            return true;
        }

        Debug.Log($"{itemType.name} not found in inventory.");
        return false;
    }

    public void TryMoveItem(int referencedSlot, int slot)
    {
        Debug.Log("TryMoveItenm"); 
        //gucken ob an der stelle schon was ist, wenn ja dann tauschen 
         //ansonsten

         if (!ItemDictionary.ContainsKey(slot))
         {
             ItemStack stack = ItemDictionary[referencedSlot];
             //ItemDictionary[slot] = ItemDictionary[referencedSlot];
             ItemDictionary.Add(slot,stack);
             ItemDictionary.Remove(referencedSlot);
             SlotDictionary[stack.ItemType] = slot;

             InventoryGUI.Instance.GetView(referencedSlot).SetReferencedSlot(slot);
             InventoryGUI.Instance.MoveViewDict(referencedSlot, slot); //DAS FEHLT NLOCH FÜR DEN ELSE BRANCH WENN MAN ACTUALLY SWAPPT
             InventoryGUI.Instance.UpdateView();    
         }
         else
         {
             //nochmal neu machen damit es mit add und remove ist
             //save dragging item to temp
             ItemStack s = new ItemStack();
             s.ItemType = ItemDictionary[referencedSlot].ItemType;
             s.Amount = ItemDictionary[referencedSlot].Amount;
             
             //swap slot item to drag origin slot
             ItemDictionary[referencedSlot] = ItemDictionary[slot];
             InventoryGUI.Instance.GetView(slot).SetReferencedSlot(referencedSlot);
             
             //put saved item into new slot
             ItemDictionary[slot] = s;
             InventoryGUI.Instance.GetView(referencedSlot).SetReferencedSlot(slot);
         }
         InventoryGUI.Instance.UpdateView();
    }

    public List<ItemStack> GetStacks()
    {
        return ItemDictionary.Values.ToList();
    }
    

    public bool HasItem(ItemSO itemType)
    {
        return SlotDictionary.ContainsKey(itemType);
    }

    public int GetItemCount()
    {
        return ItemDictionary.Count;
    }

    public void ClearInventory()
    {
        ItemDictionary.Clear();
        SlotDictionary.Clear();
        Debug.Log("Inventory cleared.");
    }
}
