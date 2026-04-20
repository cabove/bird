using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BirdVisualAnimator : MonoBehaviour
{
    [Header("Run Frames")]
    public Sprite[] runFrames;

    [Header("Jump Frames")]
    public Sprite[] jumpFrames;

    [Header("Animation Speeds")]
    public float runAnimationSpeed = 8f;
    public float jumpAnimationSpeed = 12f;

    [Header("References")]
    public Transform playerTransform;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.3f;

    private SpriteRenderer sr;
    private float animTimer;
    private int currentFrame;
    private bool wasGrounded;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        wasGrounded = IsGrounded();
        currentFrame = 0;
        animTimer = 0f;
    }

    void Update()
    {
        bool grounded = IsGrounded();

        if (grounded != wasGrounded)
        {
            currentFrame = 0;
            animTimer = 0f;
            wasGrounded = grounded;
        }

        if (grounded)
        {
            AnimateFrames(runFrames, runAnimationSpeed, true);
        }
        else
        {
            AnimateFrames(jumpFrames, jumpAnimationSpeed, false);
        }
    }

    void AnimateFrames(Sprite[] frames, float speed, bool loop)
    {
        if (frames == null || frames.Length == 0)
            return;

        if (frames.Length == 1)
        {
            sr.sprite = frames[0];
            return;
        }

        animTimer += Time.deltaTime;
        float frameDuration = 1f / speed;

        if (animTimer >= frameDuration)
        {
            animTimer -= frameDuration;
            currentFrame++;

            if (loop)
            {
                if (currentFrame >= frames.Length)
                    currentFrame = 0;
            }
            else
            {
                if (currentFrame >= frames.Length)
                    currentFrame = frames.Length - 1;
            }
        }

        sr.sprite = frames[currentFrame];
    }

    bool IsGrounded()
    {
        if (playerTransform == null)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(
            playerTransform.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        return hit.collider != null;
    }
}
