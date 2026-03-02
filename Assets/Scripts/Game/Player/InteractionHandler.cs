using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler : MonoBehaviour
{
    private List<GameObject> interactables;
    public GameObject indicatorPrefab;
    public Color selectColor;
    public Color farColor;
    private Dictionary<GameObject, GameObject> interactionInstances;
    private bool canInteract = true;
    private GameObject selectedObj;
    private PlayerController2D player;
    private void Awake()
    {
        interactables = new List<GameObject>();
        interactionInstances = new Dictionary<GameObject, GameObject>();
        player = GetComponentInParent<PlayerController2D>();
    }
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            if (!interactables.Contains(collision.gameObject))
            {
                interactables.Add(collision.gameObject);
                GameObject indicator = Instantiate(indicatorPrefab, collision.gameObject.transform.position, Quaternion.identity);
                interactionInstances.Add(collision.gameObject, indicator);
                MarkClosestInteractable();
            }
        }
    }
    public void Interact()
    {
        if (canInteract)
        {
            if (selectedObj != null)
            {
                IInteractable[] allItems = selectedObj.GetComponents<IInteractable>();
                foreach (var item in allItems)
                {
                    item?.Interact(player);
                }
                //IInteractable interactable = selectedObj.GetComponent<IInteractable>();
                //interactable?.Interact(player);
            }
        }
    }
    private void FixedUpdate()
    {
        MarkClosestInteractable();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            if (interactables.Contains(collision.gameObject))
            {
                if (interactionInstances.ContainsKey(collision.gameObject))
                {
                    Destroy(interactionInstances[collision.gameObject]);
                    interactionInstances.Remove(collision.gameObject);
                }
                interactables.Remove(collision.gameObject);
                MarkClosestInteractable();
            }
            if (selectedObj == collision.gameObject)
                selectedObj = null;
        }
    }
    public void MarkClosestInteractable()
    {
        if (interactables.Count == 1)
        {
            interactionInstances[interactables[0]].GetComponent<SpriteRenderer>().color = selectColor;
            selectedObj = interactables[0];
        }
        else if (interactables.Count > 1)
        {
            GameObject closesObj = interactables[0];
            float closestDist = Vector3.Distance(transform.position, closesObj.transform.position);
            foreach (var item in interactables)
            {
                interactionInstances[item].GetComponent<SpriteRenderer>().color = farColor;
            }
            foreach (var item in interactables)
            {
                float newDist = Vector3.Distance(transform.position, item.transform.position);
                if (newDist < closestDist)
                {
                    closestDist = newDist;
                    closesObj = item;
                }
            }
            selectedObj = closesObj;
            interactionInstances[closesObj].GetComponent<SpriteRenderer>().color = selectColor;
        }
    }
}