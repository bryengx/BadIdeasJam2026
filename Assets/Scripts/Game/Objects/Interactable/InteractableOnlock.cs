using UnityEngine;

public class InteractableOnlock : MonoBehaviour, IInteractable
{
    public InventoryItem keyItem;
    [SerializeField] private AudioClip[] unlockedClips;
    [SerializeField] private AudioClip[] lockedClips;

    public void Interact(PlayerController2D player)
    {
        AudioSource playerSource = player.GetComponent<AudioSource>();
        if (player.HasItem(keyItem))
        {
            PlayRandomClip(playerSource, unlockedClips);
            player.RemoveItem(keyItem);
            Destroy(gameObject);
        }
        else
        {
            PlayRandomClip(playerSource, lockedClips);
        }
    }
    private void PlayRandomClip(AudioSource source, AudioClip[] clips)
    {
        if (source == null || clips == null || clips.Length == 0) return;
        source.pitch = Random.Range(0.9f, 1.1f);
        int index = Random.Range(0, clips.Length);
        source.PlayOneShot(clips[index], 0.4f);
    }
}
