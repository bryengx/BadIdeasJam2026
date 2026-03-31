using UnityEngine;

public class SwapGravity : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip creatureClip;
    public void Interact(PlayerController2D player)
    {
        if (audioSource != null && creatureClip != null)
        {
            audioSource.PlayOneShot(creatureClip, 0.6f);
        }
        player.revertGravity = !player.revertGravity;
    }
}
