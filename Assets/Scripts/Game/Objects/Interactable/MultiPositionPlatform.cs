using UnityEngine;
using System.Collections;

public class MultiPositionPlatform : MonoBehaviour
{
    public Transform[] positions;
    public float moveSpeed = 2.5f;
    bool isMoving = false;
    Transform currentPoint;
    Transform nextPoint;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] moveClips;
    private float fadeOutTime = 0.2f;
    private Coroutine fadeCoroutine;
    private bool wasMoving = false;
    void Start()
    {
        wasMoving = isMoving;
        if (audioSource != null) 
        {
            audioSource.Stop(); 
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving && !wasMoving)
        {
            PlayRandomMoveClip();
        }
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, nextPoint.position) > moveSpeed * Time.deltaTime)
            {
                transform.position += (nextPoint.position - transform.position).normalized * moveSpeed * Time.deltaTime;
            }
            else
            {
                currentPoint = nextPoint;
                isMoving = false;
                StopMoveClip();
            }
        }
        wasMoving = isMoving;
    }
    public void StartMoving(int posIdx)
    {
        nextPoint = positions[posIdx];
        isMoving = true;
    }
    private void PlayRandomMoveClip()
    {
        if (audioSource == null || moveClips == null || moveClips.Length == 0) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = 0.525f;
        audioSource.clip = moveClips[Random.Range(0, moveClips.Length)];
        audioSource.Play();
    }
    private void StopMoveClip()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeOutAndStop());
        }
    }
    private IEnumerator FadeOutAndStop()
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeOutTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
