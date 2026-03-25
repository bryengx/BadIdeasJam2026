using UnityEngine;

public class InteractableStartDialog : MonoBehaviour, IInteractable
{
    public Dialog dialog;
    public DialogOnTrigger.SpeakerInfo[] speakerInfo;

    public void Interact(PlayerController2D player)
    {
        DialogOnTrigger.OnTriggerDialog(null, speakerInfo, false);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
