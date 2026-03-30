using UnityEngine;
using UnityEngine.SceneManagement;
public class Sleep : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject dark;
    [SerializeField] private string sceneName;
    [SerializeField] private float waitBeforLoad = 2f;
    [SerializeField] private AudioClip sleepClip;

    public void Interact(PlayerController2D player)
    {
        PlaySleepSound(player);
        dark.SetActive(true);
        Invoke(nameof(Load), waitBeforLoad);
    }
    private void Load()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    private void PlaySleepSound(PlayerController2D player)
    {
        if (sleepClip == null) return;
        AudioSource source = player.GetComponent<AudioSource>();
        if (source != null)
        {
            source.volume = 1.0f;
            source.PlayOneShot(sleepClip, 0.8f);
        }
    }
}
