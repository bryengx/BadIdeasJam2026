using UnityEngine;
using UnityEngine.InputSystem;

public class WashDishes : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject sinkView;
    [SerializeField] private LayerMask plateLayer;
    [SerializeField] private CompleteTask completeTask;
    [SerializeField] private Transform dirtyPlatesParent;
    [SerializeField] private Transform cleanPlateStack;
    [SerializeField] private Transform plateParent;
    [SerializeField] private GameObject dirts;
    [SerializeField] private GameObject instructions;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] pickUpPlates;
    [SerializeField] private AudioClip[] putDownPlates;
    private Transform dirtParent;

    private Camera mainCam;
    private bool isWashing = false;
    private bool finished = false;

    private void Start()
    {
        mainCam = Camera.main;
        instructions.SetActive(sinkView.activeSelf);
    }
    public void Interact(PlayerController2D player)
    {
        sinkView.SetActive(!sinkView.activeSelf);
        instructions.SetActive(sinkView.activeSelf);
        player.canMove = !sinkView.activeSelf;
    }

    private void Update()
    {
        if (finished) return;

        if(dirtParent != null && isWashing)
        {
            if(dirtParent.childCount == 0)
            {
                StackPlates(dirtParent.parent.GetComponent<SpriteRenderer>().color);
                Destroy(dirtParent.parent.gameObject);
                isWashing = false;
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && isWashing == false)
        {
            Vector2 mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.value);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos, plateLayer);
            if (hitCollider != null)
            {
                PlayRandomPickUp();
                SpriteRenderer rend = hitCollider.GetComponent<SpriteRenderer>();
                rend.sortingOrder++;

                hitCollider.transform.SetParent(plateParent);
                hitCollider.transform.localPosition = Vector3.zero;
                hitCollider.transform.localRotation = Quaternion.identity;
                hitCollider.transform.localScale = Vector3.one;

                dirtParent = Instantiate(dirts).transform;
                dirtParent.gameObject.SetActive(true);
                dirtParent.SetParent(hitCollider.transform);
                dirtParent.localPosition = Vector3.zero;
                isWashing = true;
                Destroy(hitCollider);
            }
        }
        if (dirtyPlatesParent.childCount == 0 && isWashing == false)
        {
            finished = completeTask.TaskComplete();
        }
    }
    private void StackPlates(Color color)
    {
        PlayRandomPutDown();
        foreach(Transform plate in cleanPlateStack)
        {
            if (plate.gameObject.activeSelf == false)
            {
                SpriteRenderer rend = plate.gameObject.GetComponent<SpriteRenderer>();
                rend.color = color;
                plate.gameObject.SetActive(true);
                break;
            }
        }
    }
    private void PlayRandomPickUp()
    {
        if (pickUpPlates.Length > 0)
        {
            int randomIndex = Random.Range(0, pickUpPlates.Length);
            audioSource.PlayOneShot(pickUpPlates[randomIndex], 0.85f);
        }
    }

    private void PlayRandomPutDown()
    {
        if (putDownPlates.Length > 0)
        {
            int randomIndex = Random.Range(0, putDownPlates.Length);
            audioSource.PlayOneShot(putDownPlates[randomIndex], 0.9f);
        }
    }
}
