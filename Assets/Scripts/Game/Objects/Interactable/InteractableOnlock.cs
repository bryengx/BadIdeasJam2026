using UnityEngine;

public class InteractableOnlock : MonoBehaviour, IInteractable
{
    public InventoryItem keyItem;

    public void Interact(PlayerController2D player)
    {
        if (player.HasItem(keyItem))
        {
            player.RemoveItem(keyItem);
            Destroy(gameObject);
        }
    }
}
