using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Bounds (World X)")]
    public float minX = 0f;
    public float maxX = 50f;

    [Header("Follow Settings")]
    public float smoothSpeed = 10f;

    Camera cam;
    float halfWidth;

    void Awake()
    {
        cam = GetComponent<Camera>();

        if (!cam.orthographic)
        {
            Debug.LogWarning("CameraFollow2D works best with Orthographic camera.");
        }

        CalculateHalfWidth();
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        FollowTarget();
    }

    void CalculateHalfWidth()
    {
        halfWidth = cam.orthographicSize * cam.aspect;
    }

    void FollowTarget()
    {
        float targetX = target.position.x;

        // Clamp so camera edges never go beyond bounds
        float clampedX = Mathf.Clamp(targetX, minX + halfWidth, maxX - halfWidth);

        Vector3 desiredPosition = new Vector3(clampedX, transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}