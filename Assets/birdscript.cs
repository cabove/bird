using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpForce = 8f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool isGrounded = IsGrounded();
        
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            Jump();
        }
    }

    bool IsGrounded()
    {
        // Cast a ray downward to check if touching ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}