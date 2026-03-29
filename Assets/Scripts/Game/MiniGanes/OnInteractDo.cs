using UnityEngine;
using UnityEngine.Events;

public class OnInteractDo : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private bool reapeat = true;

    private bool interacted;

    public void Interact(PlayerController2D player)
    {
        if (interacted && reapeat == false) return;
        interacted = true;
        onInteract?.Invoke();
    }
}
