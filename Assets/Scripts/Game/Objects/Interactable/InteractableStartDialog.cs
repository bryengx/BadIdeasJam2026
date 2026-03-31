using UnityEngine;

public class InteractableStartDialog : MonoBehaviour, IInteractable, IDialogAdditionalActions
{
    public Dialog.DialogInfo[] speakerInfo;
    private PlayerController2D playerObj;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip monsterClip;
    private bool hasPlayedSound = false;

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
        if (!hasPlayedSound && audioSource != null && monsterClip != null)
        {
            audioSource.PlayOneShot(monsterClip, 0.325f);
            hasPlayedSound = true;
        }
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
