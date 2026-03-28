using System.Collections;
using UnityEngine;

public class OnEnterEndGameOutside : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float startZoom = 6;
    public float endZoom = 10;
    public float waitTime = 4;
    public float time;
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerController2D player = other.GetComponent<PlayerController2D>();
            if (player != null)
            {
                player.canMove = false;
                StartCoroutine(PanCamera());
            }
        }
    }
    IEnumerator PanCamera()
    {
        CameraZoom.instance.ChangeZoom(endZoom, waitTime);
        CameraZoom.instance.ChangePosition(endPoint.position, waitTime);
        yield return new WaitForSeconds(waitTime * 2);

        ScreenFade.FadeOut(waitTime * 2);
        yield return new WaitForSeconds(waitTime * 2);
        MainMenuUI.instance.Show("Outside Ending");
    }
}
