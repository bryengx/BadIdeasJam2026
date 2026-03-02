using UnityEngine;

public class ChangeSize : MonoBehaviour, IInteractable
{
    public float newSize;
    public void Interact(PlayerController2D player)
    {
        player.transform.localScale = Vector3.one * newSize;
        //player.GetComponent<CapsuleCollider2D>().size = new Vector2(newSize, newSize * 2);
    }
}
