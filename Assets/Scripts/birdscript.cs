using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class birdscript : MonoBehaviour
{
    public AudioSource drumSound;
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.3f;

    private Rigidbody2D rb;

    public bool allowAutoMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Do not auto-move horizontally here.
        // LineManager will control the X position.
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            if (drumSound != null)
            {
                drumSound.Play();
            }
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        return hit.collider != null;
    }
}