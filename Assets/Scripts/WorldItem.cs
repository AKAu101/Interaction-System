using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemSO itemType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp()
    {
        Inventory.Instance.AddItem(itemType);
    }
}
