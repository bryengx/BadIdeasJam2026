using UnityEngine;
using System.Collections;

public class AmbienceAudioManager : MonoBehaviour
{
    public static AmbienceAudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource sourceA;
    public AudioSource sourceB;

    [Header("Ambience Clips")]
    public AudioClip roomAmbience; 
    public AudioClip caveAmbience;

    [Header("Settings")]
    public float fadeTime = 0.5f;
    private AudioSource activeSource;
    private string currentTag = "";

    void Awake()
    {
        Instance = this;
        activeSource = sourceA;
    }

    void Start()
    {
        PlayerAudio player = Object.FindFirstObjectByType<PlayerAudio>();

        if (player != null)
        {
                ChangeAmbience(player.currentRoomTag);
        }
    }

    public void ChangeAmbience(string newTag)
    {
        if (currentTag == newTag) return;
        currentTag = newTag;

        AudioClip nextClip = null;
        if (newTag == "Room") nextClip = roomAmbience;
        else if (newTag == "Cave") nextClip = caveAmbience;

        if (nextClip != null)
        {
            StopAllCoroutines(); 
            StartCoroutine(Crossfade(nextClip));
        }
    }

    IEnumerator Crossfade(AudioClip nextClip)
    {
        AudioSource newSource = (activeSource == sourceA) ? sourceB : sourceA;
        
        newSource.clip = nextClip;
        newSource.volume = 0;
        newSource.Play();

        float timer = 0;
        float startVol = activeSource.volume;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float percent = timer / fadeTime;

            newSource.volume = percent;        
            activeSource.volume = startVol * (1 - percent);
            yield return null;
        }

        activeSource.Stop();
        activeSource = newSource;
    }
}
