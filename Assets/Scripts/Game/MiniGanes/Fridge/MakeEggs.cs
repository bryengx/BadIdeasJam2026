using UnityEngine;

public class MakeEggs : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject stoveUI;
    [SerializeField] private CompleteTask completeTask;

    [SerializeField] private Transform placePan;

    [SerializeField] private Dialog.DialogInfo speaker;
   [HideInInspector] public GameObject eggs;
   [HideInInspector] public GameObject pan;
    private bool placedPan;
    private bool stoveShowing = false;
    public void Interact(PlayerController2D player)
    {
        if(pan == null)
        {
            speaker.text[0].text = "I need to find the pan first.";
            SayMissingStuff();
            return;
        }else if(eggs == null)
        {
            speaker.text[0].text = "I need some eggs now.";
            SayMissingStuff();
        }

        if(placedPan == false)
        {
            placedPan = true;
            pan.transform.position = placePan.position;
        }

        if(pan != null && eggs != null)
        {
            completeTask.TaskComplete();

            stoveShowing = !stoveShowing;
            ToggleStove();
        }
        player.canMove = !stoveShowing;
    }
    private void ToggleStove()
    {
        stoveUI.SetActive(stoveShowing);
    }
    private void SayMissingStuff()
    {
        Dialog.CallDialog(speaker, true);
    }
}
