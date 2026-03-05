using UnityEngine;

public class OnEnterMoveByVector : MonoBehaviour
{
    public Vector3 moveTo;
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerController2D player = other.GetComponent<PlayerController2D>();
            if (player != null)
                player.transform.position += moveTo;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + moveTo);
    }
}
