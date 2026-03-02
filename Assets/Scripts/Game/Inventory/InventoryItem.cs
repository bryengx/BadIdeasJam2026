using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Game/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Optional")]
    public bool isStackable = true;
}