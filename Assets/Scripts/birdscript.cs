using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class birdscript : MonoBehaviour
{
    public float lineWidth = 16f;
    public float timeToCrossLine = 13.333f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;

    private Rigidbody2D rb;
    private float moveSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = lineWidth / timeToCrossLine;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }
}