using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player
    public float jumpForce = 10f; // Force applied when jumping
    public float jumpBufferTime = 0.2f; // Time window to buffer jumps
    public Transform groundCheck; // Empty GameObject to mark the player's feet
    public float groundCheckRadius = 0.2f; // Radius for ground detection
    public LayerMask groundLayer; // Layer to identify ground objects

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private bool facingRight = true; // Track the direction the player is facing
    private float lastJumpInputTime = -1f; // Tracks the last time jump was pressed
    private float lastGroundedTime = -1f; // Tracks the last time the player was grounded
    private bool isMovementEnabled = true; // Whether the player can move

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void Update()
    {
        if (isMovementEnabled)
        {
            // Use Arrow Keys for Horizontal Movement
            float moveInput = 0f;

            if (Input.GetKey(KeyCode.A))
            {
                moveInput = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveInput = 1f;
            }

            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            // Flip the player to face the direction of movement
            if (moveInput > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveInput < 0 && facingRight)
            {
                Flip();
            }

            // Handle Jump Input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                lastJumpInputTime = Time.time; // Record the time of jump input
            }

            // Update last grounded time
            if (IsGrounded())
            {
                lastGroundedTime = Time.time; // Update the last grounded time
            }

            // Attempt to jump if the player is grounded
            if (IsGrounded() && Time.time - lastJumpInputTime <= jumpBufferTime)
            {
                Jump();
                lastJumpInputTime = -1f; // Reset the buffer after jumping
            }
        }
        else
        {
            // Stop the player's velocity completely
            rb.linearVelocity = Vector2.zero;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // External controls for enabling/disabling movement
    public void EnableMovement(bool enable)
    {
        isMovementEnabled = enable;

        // If disabling movement, also reset velocity
        if (!enable)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
