using UnityEngine;
using UnityEngine.InputSystem;

public class TakeEggs : MonoBehaviour
{
    [SerializeField] private MakeEggs makeEggs;
    [SerializeField] private bool isEgg = true;
    [SerializeField] private LayerMask fridgeItemsLayer;
    private DialogOnTrigger dialogTrigger;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.value;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);

            Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos, fridgeItemsLayer);
            if (hitCollider != null)
            {
                TakeEggs eggs = hitCollider.gameObject.GetComponent<TakeEggs>();
                if (eggs != null)
                {
                    if (eggs == this)
                    {
                        Debug.Log(hitCollider.gameObject.name);
                        if(isEgg == false)
                        {
                            dialogTrigger?.TriggerDialog();
                        }
                        else
                        {
                            Debug.Log("I have eggs!");
                            makeEggs.eggs = gameObject;
                            gameObject.SetActive(false);
                        }
                            
                    }
                }
            }
        }
    }
}
