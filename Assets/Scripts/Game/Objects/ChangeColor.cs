using UnityEngine;

public class ChangeColor : MonoBehaviour, IInteractable
{
    public Color[] colors;
    int idx;
    private void Start()
    {
        idx = 0;
        GetComponentInChildren<SpriteRenderer>().color = colors[idx];
    }
    public void Interact(PlayerController2D player)
    {
        idx++;
        idx %= colors.Length;

        GetComponentInChildren<SpriteRenderer>().color = colors[idx];
    }
}
