using UnityEngine;

public class birdscript2 : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpForce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("SPACE PRESSED");
            Jump();
        }
    }

    void Jump()
    {
        // Reset vertical movement and apply jump
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}