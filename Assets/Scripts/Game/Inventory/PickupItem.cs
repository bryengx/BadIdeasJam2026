using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickupItem : MonoBehaviour
{
    public InventoryItem item;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.AddItem(item, amount);
            Destroy(gameObject);
        }
    }
}
