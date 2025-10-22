using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public Sprite icon;
    public new string name;
    public string description;
    public bool isConsumable;
}