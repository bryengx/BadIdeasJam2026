using UnityEngine;
using System.Collections;

public class PhoneAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip phonePickUpClip;
    [SerializeField] private AudioClip phoneHangUpClip;
    [SerializeField] private float pickupDelay = 0.5f;
    [SerializeField] private float hangupDelay = 0.5f;

   public void PlayPickUp()
    {
        StopAllCoroutines();
        StartCoroutine(DelayedPlay(phonePickUpClip, pickupDelay));
    }

    public void PlayHangUp()
    {
        StartCoroutine(DelayedPlay(phoneHangUpClip, hangupDelay));
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
