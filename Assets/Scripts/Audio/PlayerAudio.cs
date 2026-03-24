using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    private PlayerController2D playerController;
    private bool wasGrounded;
    private float lastVelocityY;

    [Header("Footstep Clips")]
    public AudioClip[] woodWalk;
    public AudioClip[] dirtWalk;

    [Header("Jump Clips")]
    public AudioClip[] woodJump;
    public AudioClip[] dirtJump;

    [Header("Land Clips")]
    public AudioClip[] woodLand;
    public AudioClip[] dirtLand;

    [Header("Randomization Settings")]
    [Range(0f, 1f)] public float baseVolume = 0.8f; 
    [Range(0f, 0.2f)] public float volumeVariation = 0.1f;
    [Range(0.8f, 1.5f)] public float minPitch = 0.95f;       
    [Range(0.8f, 1.5f)] public float maxPitch = 1.15f;

    private string currentRoomTag = "Wood"; 

    void Awake()
    {
        playerController = GetComponent<PlayerController2D>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        wasGrounded = true;
    }

    void Update()
    {
        if (playerController == null) return;
        float currentVelocityY = GetComponent<Rigidbody2D>().linearVelocity.y;

        // Land
        if (!wasGrounded && playerController.isGrounded)
        {
            if (currentRoomTag == "Wood") PlayRandom(woodLand);
            else if (currentRoomTag == "Dirt") PlayRandom(dirtLand);
        }

        // Ground jump
        bool groundJump = wasGrounded && !playerController.isGrounded && currentVelocityY > 0.1f;
        // Wall jump
        bool wallJump = !playerController.isGrounded && lastVelocityY <= 0.5f && currentVelocityY > 0.8f;
        
        if (groundJump || wallJump)
        {
            if (currentRoomTag == "Wood") PlayRandom(woodJump);
            else if (currentRoomTag == "Dirt") PlayRandom(dirtJump);
        }

        wasGrounded = playerController.isGrounded;
        lastVelocityY = currentVelocityY;
    }

    public void PlayFootstep()
    {
        if (playerController != null && !playerController.isGrounded) return; 
        
        if (currentRoomTag == "Wood")
        {
            PlayRandom(woodWalk);
        }
        else if (currentRoomTag == "Dirt")
        {
            PlayRandom(dirtWalk);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Wood") || collision.CompareTag("Dirt"))
        {
            currentRoomTag = collision.tag;
        }
    }
}
