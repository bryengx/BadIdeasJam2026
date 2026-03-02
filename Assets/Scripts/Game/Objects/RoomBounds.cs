using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomBounds : MonoBehaviour
{
    public Vector2 cameraPosition;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Camera.main.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, -10);
        }
    }
}
