using System.Collections;
using UnityEngine;

public class EggBrake : SpriteDrag
{
    [SerializeField] private GameObject eggYolk;
    [SerializeField] private Sprite crackedSprite;
    [SerializeField] private float breakSPeed = 5f;
    [SerializeField] protected Dialog.DialogInfo failToBrakeText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] eggNotBrokenClips;
    [SerializeField] private AudioClip eggBrokenClip;
    [SerializeField] private MakeEggs makeEggs;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactSpeed = collision.relativeVelocity.magnitude;

        Debug.Log("Speed was: " + impactSpeed);

        if (impactSpeed >= breakSPeed)
        {
            if (eggBrokenClip != null)
            {
                AudioSource.PlayClipAtPoint(eggBrokenClip, transform.position);
            }
            StartCoroutine(BreakEgg());
            
        }
        else
        {
            PlayRandomSFX();
            Dialog.CallDialog?.Invoke(failToBrakeText, true);
        }
    }

    private IEnumerator BreakEgg()
    {
        GetComponent<SpriteRenderer>().sprite = crackedSprite;
        endDrag = true;
        if (makeEggs != null)
        {
            makeEggs.SetEggBroken(true);
        }
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
    
    private void PlayRandomSFX()
    {
        if (eggNotBrokenClips == null || eggNotBrokenClips.Length == 0 || audioSource == null) return;
        int randomIndex = Random.Range(0, eggNotBrokenClips.Length);
        AudioClip clip = eggNotBrokenClips[randomIndex];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clip);
    }
}
