using System.Collections;
using UnityEngine;

public class OnEnterDayStart : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float startZoom = 6;
    public float endZoom = 6;
    public float waitTime = 4;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerController2D player = other.GetComponent<PlayerController2D>();
            if (player != null)
            {
                StartCoroutine(PerformAction(player));
            }
        }
    }
    private IEnumerator PerformAction(PlayerController2D player)
    {

        player.canMove = false;
        ScreenFade.FadeOut(0);
        yield return new WaitForSeconds(0.5f);
        ScreenFade.FadeIn(waitTime);
        Camera.main.orthographicSize = startZoom;
        Camera.main.transform.position = startPoint.position + Vector3.back * 10;

        CameraZoom.instance.ChangeZoom(endZoom, waitTime);

        yield return new WaitForSeconds(waitTime / 2);
        CameraZoom.instance.ChangePosition(endPoint.position, waitTime / 2);

        yield return new WaitForSeconds(waitTime / 2);
        player.canMove = true;
        yield return null;
    }
}
