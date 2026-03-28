using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI instance;
    public GameObject root;
    public TextMeshProUGUI titleText;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        root.SetActive(false);
    }
    public void Show(string title)
    {
        root.SetActive(true);
        titleText.text = title;
    }
    public void ChangeScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }
}
