using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;

    bool isFacingRight = true;

void Start()
{
    rb = GetComponent<Rigidbody2D>();
}

void Update()
{
    float moveInput = Input.GetAxis("Horizontal");
    rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

    if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
    {
        Debug.Log("Jump Input Detected");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // Flip the player based on the movement direction
    if (moveInput > 0 && !isFacingRight)
    {
        Flip();
    }
    else if (moveInput < 0 && isFacingRight)
    {
        Flip();
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
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
