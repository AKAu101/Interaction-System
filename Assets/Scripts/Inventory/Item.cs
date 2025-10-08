using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Material,
    Tool,
    QuestItem
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public string interactMessage;
    [TextArea(3, 5)]
    public string description;

    [Header("Visuals")]
    public Sprite icon;
    public GameObject worldPrefab;

    [Header("Properties")]
    public ItemType type;
    public int maxStackSize = 1;
    public int value;
    public bool isPickable = true;
}
