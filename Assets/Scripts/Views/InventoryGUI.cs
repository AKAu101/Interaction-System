using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using Generals;
public class InventoryGUI : Singleton<InventoryGUI>
{
    private bool isVisible;

    [SerializeField] private GameObject wrapper;
    private Dictionary<int, GameObject> indexToSlot = new Dictionary<int, GameObject>();
    [SerializeField] List<GameObject> slotObjects;
    private Dictionary<int,ItemView> ViewDictionary = new Dictionary<int, ItemView>();

    protected override void Awake()
    {
        base.Awake();
        for(int i = 0; i < slotObjects.Count; i++)
        {indexToSlot[i] = slotObjects[i];}
        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDictionary()
    {
        foreach (ItemStack s in Inventory.Instance.GetStacks())
        {
            int slot = Inventory.Instance.SlotDictionary[s.ItemType];
            
        }
    }

    public void IntegrateView(ItemView view)
    {
        if(view == null)Debug.LogWarning("Bastard2");
        view.transform.SetParent(indexToSlot[view.ReferencedSlot].transform);
        ViewDictionary.Add(view.ReferencedSlot, view);
        
        view.transform.position = indexToSlot[view.ReferencedSlot].transform.position;
        
    }

    public ItemView GetView(int slot)
    {
        return ViewDictionary[slot];
    }

    public void MoveViewDict(int referencedSlot, int slot)
    {
        ViewDictionary[slot] = ViewDictionary[referencedSlot];
        ViewDictionary.Remove(referencedSlot);
    }

    public void UpdateView()
    {
        //matching locations to slots again
        foreach (var v in ViewDictionary.Values.ToList())
        {
            v.transform.position = indexToSlot[v.ReferencedSlot].transform.position;
            v.transform.SetParent(indexToSlot[v.ReferencedSlot].transform);
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
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                wrapper.SetActive(true);
                isVisible = true;
            }
        }

    }
}
