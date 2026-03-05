using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomBounds : MonoBehaviour
{
    public Vector2 cameraPosition;
    public bool useOwnCoord;
    public float cameraSize;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if(useOwnCoord)
                Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            else
                Camera.main.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, -10);

            Camera.main.orthographicSize = cameraSize;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 botLeft = new Vector3(transform.position.x - cameraSize * 1.8f, transform.position.y - cameraSize, 0);
        Vector3 botRight = new Vector3(transform.position.x + cameraSize * 1.8f, transform.position.y - cameraSize, 0);
        Vector3 topLeft = new Vector3(transform.position.x - cameraSize * 1.8f, transform.position.y + cameraSize, 0);
        Vector3 topRight = new Vector3(transform.position.x + cameraSize * 1.8f, transform.position.y + cameraSize, 0);
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(botLeft, topLeft);
        Gizmos.DrawLine(botRight, topRight);
    }
}
