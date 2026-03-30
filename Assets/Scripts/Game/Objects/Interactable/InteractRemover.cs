using UnityEngine;

public class InteractRemover : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip openSound;
    public GameObject removeable;
    public void Interact(PlayerController2D player)
    {
        if(removeable != null)
        {
            PlaySound(player);
            Destroy(removeable);
        } 
    }
    private void PlaySound(PlayerController2D player)
    {
        if (openSound == null) return;
        AudioSource source = player.GetComponent<AudioSource>();
        if (source != null)
        {
            source.PlayOneShot(openSound, 0.5f);
        }
    }
}
