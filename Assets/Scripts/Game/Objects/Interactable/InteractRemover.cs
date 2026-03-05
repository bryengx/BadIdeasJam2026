using UnityEngine;

public class InteractRemover : MonoBehaviour, IInteractable
{
    public GameObject removeable;
    public void Interact(PlayerController2D player)
    {
        if(removeable != null) 
            Destroy(removeable);
    }
}
