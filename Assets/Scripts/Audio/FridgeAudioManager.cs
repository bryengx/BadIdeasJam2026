using UnityEngine;
using System.Collections;

public class FridgeAudioManager : MonoBehaviour
{
    [SerializeField] private GameObject fridgeUI;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] openClips;
    [SerializeField] private AudioClip[] closeClips;

    private bool lastUIState = false;

    void Start()
    {
        if (fridgeUI != null) lastUIState = fridgeUI.activeSelf;
    }

    void Update()
    {
        if (fridgeUI == null || audioSource == null) return;

        bool currentUIState = fridgeUI.activeSelf;

        if (currentUIState != lastUIState)
        {
            if (currentUIState == true) 
                PlayRandom(openClips);
            else 
                OnFridgeClose();

            lastUIState = currentUIState;
        }
    }

    private void OnFridgeClose()
    {
        PlayRandom(closeClips);
    }

    private void PlayRandom(AudioClip[] clips)
    {
        if (clips.Length == 0 || audioSource == null) return;
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}
