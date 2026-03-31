using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class PushableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] moveClips;
    private float fadeOutTime = 0.2f;
    private Coroutine fadeCoroutine;
    bool wasMoving = false;
    public Transform[] positions;
    public float moveSpeed = 1;
    bool isMoving = false;
    Transform currentPoint;
    Transform nextPoint;
    void Start()
    {
        Transform closestPoint = positions[0];
        float closestDistance = Vector3.Distance(closestPoint.position, transform.position);
        foreach (Transform t in positions)
        {
            float distance = Vector3.Distance(t.position, transform.position);
            if (closestDistance > distance)
            {
                closestDistance = distance;
                closestPoint = t;
            }
        }
        transform.position = closestPoint.position;
        currentPoint = closestPoint;
    }
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

    public void Interact(PlayerController2D player)
    {
        if (isMoving)
            return;

        bool moveLeft = transform.position.x < player.transform.position.x;

        Transform leftNode = FindLeftNode();
        Transform rightNode = FindRightNode();

        if (moveLeft && leftNode != null)
        {
            StartMoving(leftNode);
        }
        if (!moveLeft && rightNode != null)
        {
            StartMoving(rightNode);
        }

    }
    void StartMoving(Transform target)
    {
        nextPoint = target;
        isMoving = true;
    }
    Transform FindLeftNode()
    {
        List<Transform> leftPoints = positions.Where(x => x != currentPoint && x.transform.position.x < transform.position.x).ToList();
        if(leftPoints.Count > 0)
        {
            leftPoints.Sort((a, b) => b.position.x.CompareTo(a.position.x));
            return leftPoints[0];
        }
        return null;
    }
    Transform FindRightNode()
    {
        List<Transform> rightPoints = positions.Where(x => x != currentPoint &&  x.transform.position.x > transform.position.x).ToList();
        if (rightPoints.Count > 0)
        {
            rightPoints.Sort((a, b) => a.position.x.CompareTo(b.position.x));
            return rightPoints[0];
        }
        return null;
    }
    private void PlayRandomMoveClip()
    {
        if (audioSource == null || moveClips == null || moveClips.Length == 0) return;
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = 0.5f;
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
