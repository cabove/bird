using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class birdscript : MonoBehaviour
{
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.5f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(0f, jumpForce);
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }
}