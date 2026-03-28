using UnityEngine;

public class OnEnterChangeBGM : MonoBehaviour
{
    public AudioClip newBGM;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerController2D player = other.GetComponent<PlayerController2D>();
            if (player != null)
            {
                AudioSource audio = Camera.main.GetComponent<AudioSource>();
                audio.clip = newBGM;
                audio.Play();
            }
        }
    }
}
