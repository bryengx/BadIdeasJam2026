using System.Collections;
using System.Net;
using UnityEngine;

public class InteractBedDay3End : MonoBehaviour, IInteractable
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform endLocation;
    public float startZoom = 6;
    public float endZoom = 6;
    public float waitTime = 4;
    public void Interact(PlayerController2D player)
    {
        StartCoroutine(EndDay(player));
    }
    IEnumerator EndDay(PlayerController2D player)
    {
        player.canMove = false;
        //ScreenFade.FadeOut(0);
        yield return new WaitForSeconds(0.5f);
        ScreenFade.FadeOut(waitTime);
        Camera.main.orthographicSize = startZoom;
        Camera.main.transform.position = startPoint.position + Vector3.back * 10;
        CameraZoom.instance.ChangePosition(endPoint.position, waitTime);

        CameraZoom.instance.ChangeZoom(endZoom, waitTime);

        yield return new WaitForSeconds(waitTime / 2);

        yield return new WaitForSeconds(waitTime / 2);
        player.canMove = true;
        player.transform.position = endLocation.position;
        yield return null;
        Destroy(gameObject);
    }
}
