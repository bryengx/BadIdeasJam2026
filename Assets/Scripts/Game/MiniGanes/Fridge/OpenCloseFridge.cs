using UnityEngine;

public class OpenCloseFridge : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject fridgeUI;

    private bool open = false;
    public void Interact(PlayerController2D player)
    {
        open = !open;

        ToggleView();
    }
    private void ToggleView()
    {
        fridgeUI.SetActive(open);
    }
}
