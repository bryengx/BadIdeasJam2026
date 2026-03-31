using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal targetPortal;
    bool ignoreEntry = false;
    [SerializeField] private AudioClip[] portalClips;
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (ignoreEntry)
            {
                ignoreEntry = false;
                return;
            }
            PlayerController2D player = other.GetComponent<PlayerController2D>();
            if (targetPortal != null && player != null)
            {
                PlayRandomPortal(player);
                targetPortal.ignoreEntry = true;
                player.transform.position = targetPortal.transform.position;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.25f);
        if (targetPortal != null)
            Gizmos.DrawLine(transform.position, targetPortal.transform.position);
    }
    private void PlayRandomPortal(PlayerController2D player)
    {
        if (portalClips == null || portalClips.Length == 0) return;

        AudioSource source = player.GetComponent<AudioSource>();
        if (source != null)
        {
            source.pitch = 1f;
            int randomIndex = Random.Range(0, portalClips.Length);
            source.PlayOneShot(portalClips[randomIndex], 0.3f);
        }
    }
}
