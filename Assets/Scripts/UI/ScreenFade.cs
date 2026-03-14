using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenFade : MonoBehaviour
{
    static ScreenFade instance;
    public Material fadeMaterial;
    public float step = 1f;

    private float target = 1f;
    private float current = 1f;
    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        current = Mathf.MoveTowards(current, target, step * Time.deltaTime);
        fadeMaterial.SetFloat("_Fade", current);
    }

    public static void FadeOut(float duration = 4)
    {
        instance.target = 0f;
        if (duration == 0)
        {
            instance.current = instance.target;
            instance.fadeMaterial.SetFloat("_Fade", instance.target);
        }
        else
            instance.step = 1 / duration;
    }

    public static void FadeIn(float duration = 4)
    {
        instance.target = 1f;
        if (duration == 0)
        {
            instance.current = instance.target;
            instance.fadeMaterial.SetFloat("_Fade", instance.target);
        }
        else
            instance.step = 1 / duration;
    }
}