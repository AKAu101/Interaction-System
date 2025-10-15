using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemSO itemType;

    public void PickUp()
    {
        Inventory.Instance.AddItem(itemType);
    }
}