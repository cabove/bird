using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class birdscript : MonoBehaviour
{
    public float startX = -8f;
    public float endX = 8f;
    public float timeToCrossLine = 13.333f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.5f;

    private Rigidbody2D rb;
    private float moveSpeed;

    public bool allowAutoMove = true;

    void Awake()
    {
        transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = (endX - startX) / timeToCrossLine;
    }

    void FixedUpdate()
    {
        if (!allowAutoMove)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

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