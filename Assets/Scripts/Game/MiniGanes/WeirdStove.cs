using UnityEngine;

public class WeirdStove : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject stoveUI;

    [SerializeField] private Dialog.DialogInfo speaker;

    bool b;
    public void Interact(PlayerController2D player)
    {
        if(!b)Dialog.CallDialog?.Invoke(speaker, true);
        b = true;

        stoveUI.SetActive(!stoveUI);
    }
}
