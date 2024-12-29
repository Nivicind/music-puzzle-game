using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Speed of the player
    [SerializeField] private float jumpForce = 10f; // Force applied when jumping

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private bool facingRight = true; // Track the direction the player is facing
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

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveInput = -1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
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
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
        }
        else
        {
            // Stop the player's velocity completely
            rb.linearVelocity = Vector2.zero;
        }
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

