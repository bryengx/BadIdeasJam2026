using System.Linq;
using Unity.VisualScripting;
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
            if (notify.text.Length == 0)
            {
                notify.text = notify.text.Concat(new[] { new DialogOnTrigger.Text() }).ToArray();
                // got to add "character name" and picture to Player class to we can auto fill all fields for specific character who picked the item
            }
            notify.text[0].text = "I found a " + item.name;
            DialogOnTrigger.SpeakerInfo[] d = new DialogOnTrigger.SpeakerInfo[1] { notify };

            DialogOnTrigger.OnTriggerDialog?.Invoke(d, true);
            Destroy(gameObject);
        }
    }
}
