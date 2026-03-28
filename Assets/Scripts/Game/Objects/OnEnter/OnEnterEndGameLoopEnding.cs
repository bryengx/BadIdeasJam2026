using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OnEnterEndGameLoopEnding : MonoBehaviour
{
    public GameObject whiteScreen;
    public Light2D light2d;
    bool animatnionStarted = false;
    public float endIntensity = 99;
    float addedIntensity = 0;
    public float time;
    void Start()
    {
        whiteScreen.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerController2D player = other.GetComponent<PlayerController2D>();
            if (player != null)
            {
                whiteScreen.SetActive(true);
                animatnionStarted = true;
            }
        }
    }
    void Update()
    {
        if (animatnionStarted)
        {
            addedIntensity += Time.deltaTime* endIntensity / time;
            light2d.intensity = addedIntensity;
            if (addedIntensity >= endIntensity)
            {
                animatnionStarted = false;
                MainMenuUI.instance.Show("Loop Ending");
            }
        }
    }
}
