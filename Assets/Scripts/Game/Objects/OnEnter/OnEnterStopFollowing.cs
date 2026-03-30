using UnityEngine;

public class OnEnterStopFollowing : MonoBehaviour
{
    public GameObject follower;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerController2D player = other.GetComponent<PlayerController2D>();
            if (player != null)
            {
                follower.GetComponent<FollowPlayer>().enabled = false;
                Destroy(gameObject);
            }
        }
    }
}
