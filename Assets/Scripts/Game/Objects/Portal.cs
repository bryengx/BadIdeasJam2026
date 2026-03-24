using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal targetPortal;
    bool ignoreEntry = false;
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
}
