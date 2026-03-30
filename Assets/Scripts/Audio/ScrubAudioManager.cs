using UnityEngine;
using UnityEngine.InputSystem;

public class ScrubAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] scrubClips;
    public float fadeSpeed = 30f;
    private float targetVolume = 0f;

    void Update()
    {
        bool isScrubbing = Mouse.current.leftButton.isPressed;

        if (isScrubbing)
        {
            if (!audioSource.isPlaying)
            {
                PickRandomClip();
                audioSource.Play();
            }
            targetVolume = 1f;

            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            float mouseSpeed = mouseDelta.magnitude;
            
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, 0.9f + (mouseSpeed * 0.01f), Time.deltaTime * 5f);
        }
        else
        {
            targetVolume = 0f;
            if (audioSource.volume < 0.01f) audioSource.Stop();
        }

        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime * fadeSpeed);
    }

    void PickRandomClip()
    {
        if (scrubClips != null && scrubClips.Length > 0)
        {
            int index = Random.Range(0, scrubClips.Length);
            audioSource.clip = scrubClips[index];
        }
    }
}
