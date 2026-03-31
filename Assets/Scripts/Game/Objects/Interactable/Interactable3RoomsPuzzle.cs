using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable3RoomsPuzzle : MonoBehaviour, IInteractable
{
    public Transform positionCenter;
    public Transform positionLeft;
    public Transform positionRight;
    public Transform selectedPosition;
    public string posTextLeft;
    public string posTextRight;
    public string posTextCenter;
    public GameObject pictureFrame;
    public MultiPositionPlatform[] plaforms;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] frameMoveClips;
    void Start()
    {
        if(plaforms.Length > 0)
            MovePlatformsToPosition();

        if(selectedPosition != null)
            pictureFrame.transform.position = selectedPosition.position;
    }
    public void Interact(PlayerController2D player)
    {
        string statement = "A picture frame ";
        if (selectedPosition == positionCenter)
            statement += "in the middle of the table";
        else if (selectedPosition == positionRight)
            statement += "on the right of the table";
        else if (selectedPosition == positionLeft)
            statement += "on the left of the table";

        List <ChoiceOptionData> choices = new();
        if(selectedPosition != positionCenter)
            choices.Add(new ChoiceOptionData(posTextCenter, () => { ChangeSetting(positionCenter); }));
        if (selectedPosition != positionRight)
            choices.Add(new ChoiceOptionData(posTextRight, () => { ChangeSetting(positionRight); }));
        if (selectedPosition != positionLeft)
            choices.Add(new ChoiceOptionData(posTextLeft, () => { ChangeSetting(positionLeft); }));
        choices.Add(new ChoiceOptionData("Leave", null));


        ChoiceWindowUI.instance.Open(statement, choices.ToArray());
    }
    void ChangeSetting(Transform newPosition)
    {
        selectedPosition = newPosition;
        pictureFrame.transform.position = selectedPosition.position;
        MovePlatformsToPosition();
        if (audioSource != null && frameMoveClips != null && frameMoveClips.Length > 0)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            int index = Random.Range(0, frameMoveClips.Length);
            audioSource.PlayOneShot(frameMoveClips[index], 0.35f);
        }
    }
    void MovePlatformsToPosition()
    {
        int idx = 0;
        if (positionCenter == selectedPosition)
            idx = 1;
        else if (positionRight == selectedPosition)
            idx = 2;

        foreach (MultiPositionPlatform platform in plaforms)
        {
            platform.StartMoving(idx);
        }
    }
}
