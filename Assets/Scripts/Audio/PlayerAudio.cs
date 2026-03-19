using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Footstep Clips")]
    public AudioClip[] woodClips;
    public AudioClip[] dirtClips;

    [Header("Randomization Settings")]
    [Range(0f, 1f)] public float baseVolume = 0.8f; 
    [Range(0f, 0.2f)] public float volumeVariation = 0.1f;
    [Range(0.8f, 1.5f)] public float minPitch = 0.95f;       
    [Range(0.8f, 1.5f)] public float maxPitch = 1.15f;

    private string currentRoomTag = "Wood"; 
    private PlayerController2D playerController;

    void Awake()
    {
        playerController = GetComponent<PlayerController2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Wood") || collision.CompareTag("Dirt"))
        {
            currentRoomTag = collision.tag;
        }
    }

    public void PlayFootstep()
    {
        if (playerController != null && !playerController.isGrounded)
        {
            return; 
        }
        if (currentRoomTag == "Wood")
        {
            PlayRandom(woodClips);
        }
        else if (currentRoomTag == "Dirt")
        {
            PlayRandom(dirtClips);
        }
    }

    private void PlayRandom(AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            // Random clips
            AudioClip clip = clips[Random.Range(0, clips.Length)];

            // Random pitch
            audioSource.pitch = Random.Range(minPitch, maxPitch);

            // Random volume
            float finalVolume = baseVolume - Random.Range(0f, volumeVariation);
            finalVolume = Mathf.Max(0.1f, finalVolume);

            audioSource.PlayOneShot(clip, finalVolume);
        }
    }
}
