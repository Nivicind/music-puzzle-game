using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float jumpBufferTime = 0.2f; // Time window to buffer jumps

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;
    private float lastJumpInputTime = -1f; // Tracks the last time jump was pressed

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveInput = 0f;

        // Allow movement only with the arrow keys
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1f;
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

        // Attempt to jump if the player is grounded and within the buffer window
        if (isGrounded && Time.time - lastJumpInputTime <= jumpBufferTime)
        {
            Jump();
            lastJumpInputTime = -1f; // Reset the buffer after jumping
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}
