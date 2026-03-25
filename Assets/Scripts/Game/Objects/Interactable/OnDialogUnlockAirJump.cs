using UnityEngine;

public class OnDialogUnlockAirJump : MonoBehaviour
{
    public void OnDialogClose()
    {
        PlayerController2D player = FindFirstObjectByType<PlayerController2D>();
        player.canAirJump = true;
    }
}
