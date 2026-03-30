using UnityEngine;

public class InteractableTeleport : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip[] doorOpenClips;
    public Transform nextLocation;
    public void Interact(PlayerController2D player)
    {
        PlayRandomDoorSound(player);
        player.transform.position = nextLocation.position;
    }
    private void PlayRandomDoorSound(PlayerController2D player)
    {
        if (doorOpenClips == null || doorOpenClips.Length == 0) return;

        AudioSource source = player.GetComponent<AudioSource>();
        if (source != null)
        {
            source.pitch = Random.Range(0.85f, 1.15f);
            source.PlayOneShot(doorOpenClips[Random.Range(0, doorOpenClips.Length)], 0.7f);
        }
    }
}
