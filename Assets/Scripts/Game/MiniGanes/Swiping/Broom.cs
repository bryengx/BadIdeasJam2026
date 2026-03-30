using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Broom : MonoBehaviour, IInteractable
{

    [SerializeField] private TextMeshProUGUI controlTutorial;
    [SerializeField] private CompleteTask completeTask;
    [SerializeField] private float swipOffset = 3f;
    [SerializeField] private float swipTime = 1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickUpClip;
    [SerializeField] private AudioClip[] swipeClips;

    private float cooldDownTimer;

    private bool swipTurn = false;
    private bool pickedUp = false;
    private bool debounce = false;
    private bool canClean = false;

    private int dirtCount = 0;

    private void Awake()
    {
        controlTutorial.gameObject.SetActive(false);
        List<Dirt> dirts = FindObjectsByType<Dirt>(FindObjectsInactive.Exclude,FindObjectsSortMode.None).ToList();
        for(int i = 0; i < dirts.Count; i++)
        {
            Dirt dirt = dirts[i];
            if (dirt.type != Dirt.DirtType.Floor)
            {
                dirts.Remove(dirt);
            }
        }
        dirtCount = dirts.Count;

        
    }

    public void Interact(PlayerController2D player)
    {
        pickedUp = true;
        if (audioSource != null && pickUpClip != null)
        {
            audioSource.PlayOneShot(pickUpClip);
        }
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;

        controlTutorial.text = "Press M1 to swip!";
        controlTutorial.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (debounce) return;

        if(cooldDownTimer > 0)
        {
            cooldDownTimer -= Time.deltaTime;
            return;
        }

        if (pickedUp)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                canClean = true;
                PlayRandomSwipe();
                StartCoroutine(Swip(swipTurn));
                swipTurn = !swipTurn;
            }
        }
    }
    private IEnumerator Swip(bool leftSwip)
    {
        cooldDownTimer = swipTime / 2f;

        float set = leftSwip ? swipOffset * -1 : swipOffset;
        Vector3 toDest = new Vector3(set, 0);
        float speed = Vector3.Distance(transform.localPosition, toDest)/swipTime;

        float elapsed = 0f;

        while (elapsed < swipTime)
        {
            elapsed += Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, toDest, speed * Time.deltaTime);
            yield return null;
        }
        canClean = false;


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canClean == false) return;

        Dirt dirt = other.gameObject.GetComponent<Dirt>();

        if(dirt != null)
        {
            if(dirt.type == Dirt.DirtType.Floor)
            {
                dirt.stain--;
                if (dirt.stain == 0)
                {
                    dirtCount--;

                    if (dirtCount == 0)
                    {
                        completeTask.TaskComplete();
                        controlTutorial.gameObject.SetActive(false);
                        Destroy(gameObject);
                    }
                }
            }
            
        }
        
    }
    private void PlayRandomSwipe()
    {
        if (audioSource == null || swipeClips.Length == 0) return;
        audioSource.pitch = Random.Range(0.85f, 1.15f);
        AudioClip clip = swipeClips[Random.Range(0, swipeClips.Length)];
        audioSource.PlayOneShot(clip);
    }
}
