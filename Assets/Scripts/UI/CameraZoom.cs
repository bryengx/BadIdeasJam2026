using System.Collections;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public static CameraZoom instance;
    private void Awake()
    {
        instance = this;
    }
    public void ChangeZoom(float newZoom, float duration)
    {
        StartCoroutine(DoZoomChange(newZoom, duration));
    }
    public void ChangePosition(Vector3 newPosition, float duration)
    {
        StartCoroutine(DoPositionChange(newPosition + Vector3.back * 10, duration));
    }
    private IEnumerator DoZoomChange(float newZoom, float fullDuration)
    {
        Camera cam = Camera.main;
        float oldZoom = cam.orthographicSize;
        float duration = 0;
        while (duration < fullDuration)
        {
            duration += Time.deltaTime;
            cam.orthographicSize = Mathf.Lerp(oldZoom, newZoom, duration / fullDuration);

            yield return null;
        }
        yield return null;
    }
    private IEnumerator DoPositionChange(Vector3 newPosition, float fullDuration)
    {
        Camera cam = Camera.main;
        Vector3 oldPosition = cam.transform.position;
        float duration = 0;
        while (duration < fullDuration)
        {
            duration += Time.deltaTime;
            cam.transform.position = Vector3.Lerp(oldPosition, newPosition, duration / fullDuration);

            yield return null;
        }
        yield return null;
    }
}
