using UnityEngine;
using UnityEngine.SceneManagement;
public class TouchBlackFigure : MonoBehaviour
{
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private string sceneName;
    [SerializeField] float speed = 1f;
    bool b;
    private GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Collider2D>().enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (b) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            b = true;
            blackScreen.SetActive(true);
            Invoke("load", 0.5f);
            
        }
    }
    void load()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position,speed*Time.deltaTime);
        speed += Time.deltaTime;
    }
}
