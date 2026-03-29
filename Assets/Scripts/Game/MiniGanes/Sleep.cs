using UnityEngine;
using UnityEngine.SceneManagement;
public class Sleep : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject dark;
    [SerializeField] private string sceneName;
    [SerializeField] private float waitBeforLoad = 2f;

    public void Interact(PlayerController2D player)
    {
        dark.SetActive(true);
        Invoke(nameof(Load), waitBeforLoad);
    }
    private void Load()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
