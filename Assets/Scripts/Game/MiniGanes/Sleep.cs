using UnityEngine;

public class Sleep : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject dark;
    [SerializeField] private AudioClip sleepClip;

    public void Interact(PlayerController2D player)
    {
        PlaySleepSound(player);
        dark.SetActive(true);
    }
    private void PlaySleepSound(PlayerController2D player)
    {
        if (sleepClip == null) return;
        AudioSource source = player.GetComponent<AudioSource>();
        if (source != null)
        {
            source.pitch = 1.0f;
            source.PlayOneShot(sleepClip);
        }
    }
}
