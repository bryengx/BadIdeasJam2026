using UnityEngine;

public class InteractableTeleport : MonoBehaviour, IInteractable
{
    public Transform nextLocation;
    public void Interact(PlayerController2D player)
    {
        player.transform.position = nextLocation.position;
    }
}
