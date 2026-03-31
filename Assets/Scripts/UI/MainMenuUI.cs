using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private AudioClip exitButtonClip;
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
        if (exitButtonClip != null)
        {
            AudioSource.PlayClipAtPoint(exitButtonClip, Camera.main.transform.position, 0.8f);
        }
        SceneManager.LoadScene(nextScene);
    }
}
