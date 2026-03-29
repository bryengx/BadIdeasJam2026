using System.Collections;
using UnityEngine;

public class EggBrake : SpriteDrag
{
    [SerializeField] private GameObject eggYolk;
    [SerializeField] private Sprite crackedSprite;
    [SerializeField] private float breakSPeed = 5f;
    [SerializeField] protected Dialog.DialogInfo failToBrakeText;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactSpeed = collision.relativeVelocity.magnitude;

        Debug.Log("Speed was: " + impactSpeed);

        if (impactSpeed >= breakSPeed)
        {
            StartCoroutine(BreakEgg());
            
        }
        else
        {
            Dialog.CallDialog?.Invoke(failToBrakeText, true);
        }
    }

    private IEnumerator BreakEgg()
    {
        GetComponent<SpriteRenderer>().sprite = crackedSprite;
        endDrag = true;
        yield return new WaitForSeconds(1f);

        bool succ = GetComponent<CompleteTask>().TaskComplete();
        if (succ)
        {
            Debug.Log("Egg broke!");
            eggYolk.SetActive(true);

            Destroy(gameObject);
        }
        else
        {
            endDrag = false;
            Debug.LogWarning("Failed to complete task. Try again");
        }
        
    }
}
