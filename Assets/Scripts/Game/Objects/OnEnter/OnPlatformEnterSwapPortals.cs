using UnityEngine;

public class OnPlatformEnterSwapPortals : MonoBehaviour
{
    public Portal sourcePortal;
    public Portal targetPortal;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            sourcePortal.targetPortal = targetPortal;
        }
    }
}
