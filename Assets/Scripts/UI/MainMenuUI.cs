using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void ChangeScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }
}
