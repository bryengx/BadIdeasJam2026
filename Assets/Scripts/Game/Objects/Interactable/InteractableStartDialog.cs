using UnityEngine;

public class InteractableStartDialog : MonoBehaviour, IInteractable, IDialogAdditionalActions
{
    public Dialog.DialogInfo[] speakerInfo;
    private PlayerController2D playerObj;

    public void AfterDialog()
    {
        Debug.Log("AfterDialog");
        playerObj.canMove = true;
        playerObj.canAirJump = true;
        StartFollowingPlayer();
    }

    public void BeforeDialog()
    {
        playerObj.canMove = false;
        Debug.Log("BeforeDialog");
    }
    void StartFollowingPlayer()
    {
        GetComponent<FollowPlayer>().enabled = true;
        gameObject.tag = "Untagged";
        Destroy(this);
    }

    public void Interact(PlayerController2D player)
    {
        playerObj = player;

        Dialog.CallDialogs?.Invoke(speakerInfo, false, BeforeDialog, AfterDialog,true);

        //dialog.StartDialog(this, speakerInfo, false);
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
