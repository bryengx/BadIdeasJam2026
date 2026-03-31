using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableChangeScene : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip[] doorOpenClips;
    public string nextScene;
    public void Interact(PlayerController2D player)
    {
        PlayRandomDoorSound(player);
        SceneManager.LoadScene(nextScene);
    }
    private void PlayRandomDoorSound(PlayerController2D player)
    {
        if (doorOpenClips == null || doorOpenClips.Length == 0) return;

        AudioSource source = player.GetComponent<AudioSource>();
        if (source != null)
        {
            source.pitch = Random.Range(0.85f, 1.15f);
            source.PlayOneShot(doorOpenClips[Random.Range(0, doorOpenClips.Length)], 0.55f);
        }
    }
}
