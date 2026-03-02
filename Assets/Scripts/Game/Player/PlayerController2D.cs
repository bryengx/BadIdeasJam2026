using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Swithes")]
    public bool canJump = true;
    public bool canAirJump = false;
    public bool canDash = false;
    public bool canWallJump = false;
    public bool canStickToWalls = false;
    public bool canClimb = false;
    public bool revertGravity = false;

    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 60f;
    public float airAcceleration = 5f;
    public float maxFallSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 16f;
    public int maxJumps = 2;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    public float jumpCutMultiplier = 0.4f;   // 0.3–0.6 is typical
    public float fallGravityMultiplier = 1.5f; // optional, improves feel

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;

    [Header("Wall")]
    public Vector2 wallJumpForce = new Vector2(12f, 16f);
    public float climbAssistForce = 8f;
    public float wallCoyoteTime = 0.12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Transform groundCheckFront;
    public float groundCheckRadius = 0.2f;

    [Header("Wall Checks")]
    public Transform wallCheckTop;
    public Transform wallCheckMiddle;
    public Transform wallCheckBottomLow;
    public Transform wallCheckBottomHigh;
    public float wallCheckDistance = 0.4f;

    public LayerMask groundLayer;

    Rigidbody2D rb;
    PlayerInputHandler input;

    bool isGrounded;
    bool isDashing;
    bool isWallSticking; 
    int gravityDirection = -1;

    bool topHit, midHit, bottomLowHit, bottomHighHit;
    int wallDirection;

    float wallCoyoteTimer;
    Vector2 lastWallNormal;

    float lastGroundedTime;
    float lastJumpPressedTime;

    float dashTimer;
    float dashCooldownTimer;
    float dashDirection;
    bool dashRequested;

    float wallStickDisableTimer;
    public float wallStickDisableDuration = 0.1f;

    int jumpCount;
    float defaultGravity;
    public TextMeshPro debugText;
    InteractionHandler interactionHandler;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputHandler>();
        interactionHandler = GetComponentInChildren<InteractionHandler>();
        defaultGravity = rb.gravityScale;
    }

    void Update()
    {
        CheckCollisions();
        HandleTimers();
        HandleWallLogic();
        HandleDash();
        HandleInteraction();
    }

    void FixedUpdate()
    {
        UpdateGravityState();

        HandleJump();

        if (dashRequested)
        {
            StartDash();
            dashRequested = false;
        }

        if (isDashing)
        {
            PerformDash();
            return;
        }

        HandleMovement();
        HandleJumpCut();
        HandleBetterGravity();
    }
    void UpdateGravityState()
    {
        gravityDirection = revertGravity ? -1 : 1;
        defaultGravity = Mathf.Abs(defaultGravity) * gravityDirection;
        //rb.gravityScale = Mathf.Abs(defaultGravity) * gravityDirection;

        // Flip player vertically
        Vector3 scale = transform.localScale;
        scale.y = Mathf.Abs(scale.y) * (revertGravity ? -1 : 1);
        transform.localScale = scale;
    }
    void HandleInteraction()
    {
        if (input.ActionPressed)
        {
            interactionHandler.Interact();
        }
    }

    void CheckCollisions()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle(groundCheckFront.position, groundCheckRadius, groundLayer);

        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        wallDirection = transform.localScale.x > 0 ? 1 : -1;

        topHit = Physics2D.Raycast(wallCheckTop.position, dir, wallCheckDistance, groundLayer);
        midHit = Physics2D.Raycast(wallCheckMiddle.position, dir, wallCheckDistance, groundLayer);
        bottomLowHit = Physics2D.Raycast(wallCheckBottomLow.position, dir, wallCheckDistance, groundLayer);
        bottomHighHit = Physics2D.Raycast(wallCheckBottomHigh.position, dir, wallCheckDistance, groundLayer);

        if (isGrounded)
        {
            lastGroundedTime = coyoteTime;
            jumpCount = 0;
        }

        if (input.JumpPressed)
            lastJumpPressedTime = jumpBufferTime;
    }

    void HandleTimers()
    {
        lastGroundedTime -= Time.deltaTime;
        lastJumpPressedTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        wallCoyoteTimer -= Time.deltaTime;
        wallStickDisableTimer -= Time.deltaTime;

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
                isDashing = false;
        }
    }

    void HandleMovement()
    {
        float targetSpeed = input.MoveInput.x * moveSpeed;
        float accel = isGrounded ? acceleration : airAcceleration;

        float speedDif = targetSpeed - rb.linearVelocity.x;
        float movement = speedDif * accel * Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x + movement, rb.linearVelocity.y);

        if (input.MoveInput.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(input.MoveInput.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void HandleWallLogic()
    {
        bool pressingTowardWall =
            input.MoveInput.x != 0 &&
            Mathf.Sign(input.MoveInput.x) == wallDirection;

        bool fullyAttached = topHit && midHit && bottomLowHit;

        // If fully attached, refresh wall coyote
        if (!isGrounded && fullyAttached)
        {
            wallCoyoteTimer = wallCoyoteTime;
            lastWallNormal = new Vector2(-wallDirection, 0f);
        }

        bool canStick = canStickToWalls && !isGrounded && pressingTowardWall && fullyAttached && wallStickDisableTimer <= 0f;

        if (canStick)
        {
            bool rising = rb.linearVelocity.y * gravityDirection > 0f;

            if (rising && input.JumpHeld)
            {
                rb.gravityScale = defaultGravity;
                isWallSticking = false;
            }
            else
            {
                isWallSticking = true;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.gravityScale = 0f;
            }
        }
        else
        {
            isWallSticking = false;
            rb.gravityScale = defaultGravity;
        }

        // CLIMB ASSIST
        if (canClimb && rb.linearVelocity.y * gravityDirection < 2 && pressingTowardWall && bottomLowHit && !midHit && !topHit)
        {
            float forceMultiplier = bottomHighHit ? 2f : 1f;

            rb.AddForce(
                new Vector2(-wallDirection * 2f,
                climbAssistForce * forceMultiplier),
                ForceMode2D.Force);
        }
        debugText.text = $"is:Grounded: {isGrounded} \n" +
            $"pressingTowerdsWall: {pressingTowardWall}\n" +
            $"bottomHit: {bottomLowHit}\n" +
            $"bottomHit2: {bottomHighHit}\n" +
            $"midHit: {midHit} \n" +
            $"topHit: {topHit}";
    }

    void HandleJump()
    {
        if (isDashing)
            return;

        if (lastJumpPressedTime <= 0)
            return;

        // WALL COYOTE JUMP
        if (canWallJump && wallCoyoteTimer > 0f)
        {
            PerformWallJump();
            lastJumpPressedTime = 0;
            return;
        }

        if (canJump && lastGroundedTime > 0)
        {
            Jump(Vector2.up * gravityDirection);
            return;
        }

        if (canAirJump && jumpCount < maxJumps)
        {
            Jump(Vector2.up * gravityDirection);
        }
    }

    void PerformWallJump()
    {
        wallCoyoteTimer = 0f;

        rb.gravityScale = defaultGravity;
        rb.linearVelocity = Vector2.zero;

        Vector2 force = new Vector2(lastWallNormal.x * wallJumpForce.x, wallJumpForce.y * -gravityDirection);

        rb.AddForce(force, ForceMode2D.Impulse);

        jumpCount = 1;
        wallStickDisableTimer = wallStickDisableDuration;
        isWallSticking = false;
    }

    void Jump(Vector2 direction)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);

        jumpCount++;
        lastJumpPressedTime = 0;
    }

    void HandleDash()
    {
        if (!canDash || dashCooldownTimer > 0)
            return;

        if (input.DashPressed && !isDashing)
            dashRequested = true;
    }

    void StartDash()
    {
        isDashing = true;

        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        dashDirection = input.MoveInput.x != 0
            ? Mathf.Sign(input.MoveInput.x)
            : transform.localScale.x;

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    void PerformDash()
    {
        dashTimer -= Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

        if (dashTimer <= 0f)
            EndDash();
    }

    void EndDash()
    {
        isDashing = false;
        rb.gravityScale = defaultGravity;
    }
    void HandleJumpCut()
    {
        // If player released jump while moving upward
        if (!input.JumpHeld && rb.linearVelocity.y * gravityDirection > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * gravityDirection * jumpCutMultiplier
            );
        }
    }
    void HandleBetterGravity()
    {
        float velocityY = rb.linearVelocity.y;
        if (rb.linearVelocity.y * gravityDirection < 0f)
        {
            rb.gravityScale = defaultGravity * fallGravityMultiplier;

            if (velocityY < -maxFallSpeed)
                velocityY = -maxFallSpeed;
        }
        else if (!isWallSticking && !isDashing)
        {
            rb.gravityScale = defaultGravity;
        }
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocityY);
    }
}