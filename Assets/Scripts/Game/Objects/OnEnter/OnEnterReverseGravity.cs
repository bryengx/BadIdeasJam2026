using UnityEngine;

public class OnEnterReverseGravity : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerController2D player = other.GetComponent<PlayerController2D>();
            if (player != null)
            {
                player.revertGravity = !player.revertGravity;
            }
        }
    }
}
