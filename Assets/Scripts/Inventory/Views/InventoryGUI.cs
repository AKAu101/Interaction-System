using System.Collections.Generic;
using System.Linq;
using Generals;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryGUI : Singleton<InventoryGUI>
{
    [SerializeField] private GameObject wrapper;
    [SerializeField] private List<GameObject> slotObjects;

    [SerializeField] private GameObject itemViewPrefab;
    private bool isVisible;
    private readonly Dictionary<int, GameObject> slotIndexToContainer = new();
    public Dictionary<int, ItemView> slotToView = new();

    protected override void Awake()
    {
        base.Awake();
        for (var i = 0; i < slotObjects.Count; i++) slotIndexToContainer[i] = slotObjects[i];
    }

    private void OnEnable()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnItemAdded += HandleItemAdded;
            Inventory.Instance.OnItemRemoved += HandleItemRemoved;
            Inventory.Instance.OnItemMoved += HandleItemMoved;
            Inventory.Instance.OnItemSwapped += HandleItemSwapped;
        }
    }

    private void OnDisable()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnItemAdded -= HandleItemAdded;
            Inventory.Instance.OnItemRemoved -= HandleItemRemoved;
            Inventory.Instance.OnItemMoved -= HandleItemMoved;
            Inventory.Instance.OnItemSwapped -= HandleItemSwapped;
        }
    }

    private void HandleItemAdded(ItemSO itemType, int slot)
    {
        if (!slotToView.ContainsKey(slot))
        {
            var instance = Instantiate(itemViewPrefab);
            var view = instance.GetComponent<ItemView>();
            if (view != null)
            {
                view.Setup(itemType, slot);
                IntegrateView(view);
            }
            else
            {
                Debug.LogWarning("ItemView component not found on itemViewPrefab!");
            }
        }
    }

    private void HandleItemRemoved(ItemSO itemType, int slot)
    {
        if (slotToView.ContainsKey(slot))
        {
            var view = slotToView[slot];
            if (view != null) Destroy(view.gameObject);
            slotToView.Remove(slot);
        }
    }

    private void HandleItemMoved(int sourceSlot, int targetSlot)
    {
        MoveViewDict(sourceSlot, targetSlot);
        UpdateView();
    }

    private void HandleItemSwapped(int sourceSlot, int targetSlot)
    {
        SwapViewDictEntries(sourceSlot, targetSlot);
        UpdateView();
    }


    public void IntegrateView(ItemView view)
    {
        if (view == null)
        {
            Debug.LogWarning("ItemView is null in IntegrateView!");
            return;
        }

        view.transform.SetParent(slotIndexToContainer[view.CurrentSlotIndex].transform);
        slotToView.Add(view.CurrentSlotIndex, view);

        view.transform.position = slotIndexToContainer[view.CurrentSlotIndex].transform.position;
    }

    public ItemView GetView(int slot)
    {
        return slotToView[slot];
    }

    public void MoveViewDict(int sourceSlot, int targetSlot)
    {
        slotToView[targetSlot] = slotToView[sourceSlot];
        slotToView.Remove(sourceSlot);
        slotToView[targetSlot].SetReferencedSlot(targetSlot);
    }

    public bool SwapViewDictEntries(int sourceSlot, int targetSlot)
    {
        slotToView[sourceSlot].SetReferencedSlot(targetSlot);
        slotToView[targetSlot].SetReferencedSlot(sourceSlot);
        return slotToView.SwapEntries(sourceSlot, targetSlot);
    }

    public void UpdateView()
    {
        foreach (var view in slotToView.Values.ToList())
        {
            view.transform.position = slotIndexToContainer[view.CurrentSlotIndex].transform.position;
            view.transform.SetParent(slotIndexToContainer[view.CurrentSlotIndex].transform);
        }
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isVisible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                wrapper.SetActive(false);
                isVisible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                wrapper.SetActive(true);
                isVisible = true;
            }
        }
    }

    public bool IsInventoryVisible()
    {
        return isVisible;
    }
}