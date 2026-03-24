using UnityEngine;

public class Sleep : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject dark;

    public void Interact(PlayerController2D player)
    {
        dark.SetActive(true);
    }
}
