using UnityEngine;

public class InteractableStartDialog : MonoBehaviour, IInteractable, IDialogAdditionalActions
{
    public Dialog dialog;
    public DialogOnTrigger.SpeakerInfo[] speakerInfo;
    private PlayerController2D playerObj;

    public void AfterDialog()
    {
        Debug.Log("AfterDialog");
        playerObj.canMove = true;
        playerObj.canAirJump = true;
    }

    public void BeforeDialog()
    {
        playerObj.canMove = false;
        Debug.Log("BeforeDialog");
    }

    public void Interact(PlayerController2D player)
    {
        playerObj = player;
        dialog.StartDialog(this, speakerInfo, false);
        //DialogOnTrigger.OnTriggerDialog(null, speakerInfo, false);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
