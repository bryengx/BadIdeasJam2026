using UnityEngine;

public class TakePan : MonoBehaviour, IInteractable
{
    [SerializeField] private MakeEggs makeEggs;
    [SerializeField] private AudioClip takePanClip;
    public void Interact(PlayerController2D player)
    {
        if (takePanClip != null)
        {
            AudioSource.PlayClipAtPoint(takePanClip, transform.position);
        }
        makeEggs.pan = gameObject;
        gameObject.SetActive(false);
    }
}
