using UnityEngine;

public class InteractableStartDialog : MonoBehaviour, IInteractable
{
    public Dialog dialog;
    public DialogOnTrigger.SpeakerInfo[] message;

    public void Interact(PlayerController2D player)
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        dialog.StartDialog(message, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
