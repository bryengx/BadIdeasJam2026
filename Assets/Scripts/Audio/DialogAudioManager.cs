using UnityEngine;
using System.Collections;

public class DialogAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dialogBeginClip;
    [SerializeField] private AudioClip dialogFinishClip;
    [SerializeField] private float beginDelay = 0.5f;
    [SerializeField] private float finishDelay = 0.5f;

    public void PlayDialogBegin()
    {
        StopAllCoroutines();
        StartCoroutine(DelayedPlay(dialogBeginClip, beginDelay));
    }

    public void PlayDialogFinish()
    {
        StartCoroutine(DelayedPlay(dialogFinishClip, finishDelay));
    }

    private IEnumerator DelayedPlay(AudioClip clip, float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        PlaySound(clip);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip);
    }
}
