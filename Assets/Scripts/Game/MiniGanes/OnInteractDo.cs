using UnityEngine;
using UnityEngine.Events;

public class OnInteractDo : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private bool reapeat = true;
    [SerializeField] private AudioClip heaterSound;

    private bool interacted;

    public void Interact(PlayerController2D player)
    {
        if (interacted && reapeat == false) return;
        PlayHeaterSound(player);
        interacted = true;
        onInteract?.Invoke();
    }
    private void PlayHeaterSound(PlayerController2D player)
    {
        if (heaterSound == null) return;
        AudioSource source = player.GetComponent<AudioSource>();
        if (source != null)
        {
            source.PlayOneShot(heaterSound, 0.5f);
        }
    }
}
