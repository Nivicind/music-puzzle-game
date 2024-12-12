using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player
    public float jumpForce = 10f; // Force applied when jumping
    public float jumpBufferTime = 0.2f; // Time window to buffer jumps
    public float coyoteTime = 0.2f; // Time window to allow jumps after leaving the ground
    public Transform groundCheck; // Empty GameObject to mark the player's feet
    public float groundCheckRadius = 0.2f; // Radius for ground detection
    public LayerMask groundLayer; // Layer to identify ground objects
    public Transform leftWallCheck; // Empty GameObject to check for left walls
    public Transform rightWallCheck; // Empty GameObject to check for right walls
    public float wallCheckRadius = 0.2f; // Radius for wall detection
    public LayerMask wallLayer; // Layer to identify wall objects

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private bool facingRight = true; // Track the direction the player is facing
    private float lastJumpInputTime = -1f; // Tracks the last time jump was pressed
    private float lastGroundedTime = -1f; // Tracks the last time the player was grounded

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void Update()
    {
        float moveInput = 0f;

        // Allow movement only with the arrow keys
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1f; // Move right
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1f; // Move left
        }

        // Set velocity based on input
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            lastJumpInputTime = Time.time; // Record the time of jump input
        }

        // Update last grounded time
        if (IsGrounded())
        {
            lastGroundedTime = Time.time; // Update the last grounded time
        }

        // Attempt to jump if the player is grounded or within the coyote time window
        if ((IsGrounded() || (Time.time - lastGroundedTime <= coyoteTime)) &&
            Time.time - lastJumpInputTime <= jumpBufferTime && !IsTouchingLeftWall() && !IsTouchingRightWall())
        {
            Jump();
            lastJumpInputTime = -1f; // Reset the buffer after jumping
        }
    }

    // Reliable ground detection using Physics2D.OverlapCircle
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }

    // Check if the player is touching a wall on the left side
    bool IsTouchingLeftWall()
    {
        return Physics2D.OverlapCircle(leftWallCheck.position, wallCheckRadius, wallLayer) != null;
    }

    // Check if the player is touching a wall on the right side
    bool IsTouchingRightWall()
    {
        return Physics2D.OverlapCircle(rightWallCheck.position, wallCheckRadius, wallLayer) != null;
    }

    void Flip()
    {
        facingRight = !facingRight; // Toggle the facing direction
        Vector3 scaler = transform.localScale; // Get the current scale
        scaler.x *= -1; // Flip the x scale
        transform.localScale = scaler; // Apply the new scale
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Apply jump force
    }

    // Visualize the ground and wall checks in the editor (optional)
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green; // Color for ground check
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (leftWallCheck != null)
        {
            Gizmos.color = Color.red; // Color for left wall check
            Gizmos.DrawWireSphere(leftWallCheck.position, wallCheckRadius);
        }
    }
}