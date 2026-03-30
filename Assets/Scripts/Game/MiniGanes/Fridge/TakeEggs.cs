using UnityEngine;
using UnityEngine.InputSystem;

public class TakeEggs : MonoBehaviour
{
    [SerializeField] private MakeEggs makeEggs;
    [SerializeField] private bool isEgg = true;
    [SerializeField] private LayerMask fridgeItemsLayer;
    [SerializeField] Dialog.DialogInfo cheaseDialog;
    [SerializeField] private AudioClip takeEggClip;
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
                            Dialog.CallDialog.Invoke(cheaseDialog,false);
                        }
                        else
                        {
                            Debug.Log("I have eggs!");
                            if (takeEggClip != null)
                            {
                                AudioSource.PlayClipAtPoint(takeEggClip, transform.position);
                            }
                            makeEggs.eggs = gameObject;
                            gameObject.SetActive(false);
                        }
                        enabled = false;    
                    }
                }
            }
        }
    }
}
