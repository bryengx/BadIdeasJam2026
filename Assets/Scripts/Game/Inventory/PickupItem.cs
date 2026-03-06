using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickupItem : MonoBehaviour
{
    [SerializeField] private DialogOnTrigger.SpeakerInfo notify;
    public InventoryItem item;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.AddItem(item, amount);
            notify.text[0].text = "I found a " + item.name;
            DialogOnTrigger.SpeakerInfo[] d = new DialogOnTrigger.SpeakerInfo[1] { notify };

            DialogOnTrigger.OnTriggerDialog?.Invoke(d, true);
            Destroy(gameObject);
        }
    }
}
