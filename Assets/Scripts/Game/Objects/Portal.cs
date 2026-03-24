using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal tartgetPortal;
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

            if (tartgetPortal != null && player != null)
            {
                tartgetPortal.ignoreEntry = true;
                player.transform.position = tartgetPortal.transform.position;
            }
        }
    }
}
