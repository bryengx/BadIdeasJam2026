using UnityEngine;

public class SwapGravity : MonoBehaviour, IInteractable
{
    public void Interact(PlayerController2D player)
    {
        player.revertGravity = !player.revertGravity;
    }
}
