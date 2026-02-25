using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool DashPressed { get; private set; }

    PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx =>
        {
            JumpPressed = true;
            JumpHeld = true;
        };

        inputActions.Player.Jump.canceled += ctx =>
        {
            JumpHeld = false;
        };

        inputActions.Player.Dash.performed += ctx => DashPressed = true;
    }

    void LateUpdate()
    {
        // Clear one-frame inputs
        JumpPressed = false;
        DashPressed = false;
    }

    void OnDisable()
    {
        inputActions.Disable();
    }
}