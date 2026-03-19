using UnityEngine;

public class AnimationEventForwarder : MonoBehaviour
{
    private PlayerAudio playerAudio;

    void Awake()
    {
        playerAudio = GetComponentInParent<PlayerAudio>();
    }

    public void PlayFootstep()
    {
        if (playerAudio != null)
        {
            playerAudio.PlayFootstep();
        }
    }
}
