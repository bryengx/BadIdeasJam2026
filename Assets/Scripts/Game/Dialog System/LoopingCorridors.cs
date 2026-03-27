using UnityEngine;
using static DialogOnTrigger;
[RequireComponent(typeof(Collider2D))]
public class LoopingCorridors : MonoBehaviour
{
    [SerializeField] Dialog.DialogInfo[] dialogs;
    [SerializeField] private int maxCount = 5;
    [SerializeField] bool isHighPriority = false;

    private bool debounce = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool player = collision.gameObject.CompareTag("Player");
        if (player && debounce == false)
        {
            maxCount--;
            if (maxCount == 0)
            {
                debounce = true;
                Dialog.CallDialog?.Invoke(dialogs, isHighPriority);
            }
        }
    }
}
