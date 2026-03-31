using UnityEngine;

public class OnEnterReverseGravity : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gravityClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerController2D player = other.GetComponent<PlayerController2D>();
            if (player != null)
            {
                PlayGravityClip();
                player.revertGravity = !player.revertGravity;
            }
        }
    }
    private void PlayGravityClip()
    {
        if (audioSource == null || gravityClip == null) return;
        audioSource.pitch = Random.Range(0.85f, 1.15f);
        audioSource.PlayOneShot(gravityClip, 0.35f);
    }
}
