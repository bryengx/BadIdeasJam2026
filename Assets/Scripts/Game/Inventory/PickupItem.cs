using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickupItem : MonoBehaviour
{
    
     private DialogOnTrigger.SpeakerInfo notify;
    public InventoryItem item;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.AddItem(item, amount);
            notify.text[0].text = "I found a " + item.name;
            notify.isPlayerOnly = true;

            ShowDialog();
            Destroy(gameObject);
        }
    }
    private void ShowDialog()
    {
        DialogOnTrigger.SpeakerInfo[] d = new DialogOnTrigger.SpeakerInfo[1] { notify };
        DialogOnTrigger.OnTriggerDialog?.Invoke(d, true);
    }
}
