using UnityEngine;

public class MakeEggs : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject stoveUI;
    [SerializeField] private CompleteTask completeTask;

    [SerializeField] private Transform placePan;

    [SerializeField] private Dialog.DialogInfo speaker;
    [HideInInspector] public GameObject eggs;
    [HideInInspector] public GameObject pan;
    [SerializeField] private AudioSource stoveSource;
    [SerializeField] private AudioClip stoveGasClip;
    [SerializeField] private AudioSource sizzleSource;
    [SerializeField] private AudioClip eggSizzleClip;
    private bool isEggBroken = false;
    private bool hasCompletedTask = false;

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
            if (!hasCompletedTask)
            {
                completeTask.TaskComplete();
                hasCompletedTask = true;
            }
            stoveShowing = !stoveShowing;
            player.canMove = !stoveShowing;
            ToggleStove();
        }
        player.canMove = !stoveShowing;
    }
    public void SetEggBroken(bool broken)
    {
        isEggBroken = broken;
        if (isEggBroken && stoveShowing)
        {
            sizzleSource.Play();
        }
    }
    private void ToggleStove()
    {
        stoveUI.SetActive(stoveShowing);
        if (stoveSource != null)
        {
            if (stoveShowing)
            {
                stoveSource.clip = stoveGasClip;
                stoveSource.Play();
                sizzleSource.clip = eggSizzleClip;
                if (isEggBroken && sizzleSource != null)
                {
                sizzleSource.Play();
                }
            }
            else
            {
                stoveSource.Stop();
                sizzleSource.Stop();
            }
        }
    }
    private void SayMissingStuff()
    {
        Dialog.CallDialog(speaker, true);
    }
}
