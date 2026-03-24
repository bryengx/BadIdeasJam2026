using UnityEngine;

public class TakePan : MonoBehaviour, IInteractable
{
    [SerializeField] private MakeEggs makeEggs;
    public void Interact(PlayerController2D player)
    {
        makeEggs.pan = gameObject;
        gameObject.SetActive(false);
    }
}
