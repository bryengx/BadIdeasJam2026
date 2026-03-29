using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDeckPuzzle : MonoBehaviour, IInteractable
{
    public string statement;
    //public InventoryItem oldPicture;
    public InventoryItem newPicture;
    public GameObject oldRoomObj;
    public GameObject newRoomObj;
    public Portal sourcePortal;
    public Portal destinationPortal;

    public Dialog.DialogInfo[] lookAtPicturesDialog;
    void Start()
    {
        oldRoomObj.SetActive(true);
        newRoomObj.SetActive(false);
    }
    public void Interact(PlayerController2D player)
    {
        List<ChoiceOptionData> choices = new();
        choices.Add(new ChoiceOptionData("Look at pictures", () => { TriggerDialog(lookAtPicturesDialog); }));
        if ( player.HasItem(newPicture))
            choices.Add(new ChoiceOptionData("Swap pictures", () => { SwapPictures(player); }));
        choices.Add(new ChoiceOptionData("Leave", null));


        ChoiceWindowUI.instance.Open(statement, choices.ToArray());
    }
    void TriggerDialog(Dialog.DialogInfo[] info)
    {
        ///Use event instead of searcing for an object type since its more perfomanca

        Dialog.CallDialogs?.Invoke( info, false);
    }
    void SwapPictures(PlayerController2D player)
    {
        player.RemoveItem(newPicture);
        oldRoomObj.SetActive(false);
        newRoomObj.SetActive(true);

        // redirect room door
        sourcePortal.targetPortal = destinationPortal;
    }
}
