using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // **Handle Movement Input**
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput != 0)
        {
            Debug.Log($"Move Input Detected: {moveInput}\n" +
                             $"- Ground state: {isGrounded} \n" +
                             $"- Rigidbody velocity: {rb.linearVelocity}");
        }

        // Set velocity based on input
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // **Handle Jump Input**
        if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Jump Input Detected");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            Debug.Log("Player is Grounded");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
            Debug.Log("Player is No Longer Grounded");
        }
    }
}
