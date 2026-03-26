using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableChangeScene : MonoBehaviour, IInteractable
{
    public string nextScene;
    public void Interact(PlayerController2D player)
    {
        SceneManager.LoadScene(nextScene);
    }
}
