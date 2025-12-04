using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class CharacterController2D : MonoBehaviour
{
    public float speed = 1f;
    public float jumpHeight = 2f;
    public float gravityScale = 1f;
    public float deathY = -10f;

    private Rigidbody2D rb;
    private InputAction moveAction, jumpAction;
    private float moveDirection;
    private float jumpInputLockTime = 0f;
    public float jumpLockDuration = 0.1f;

    [Header("Ground Detection")]
    public bool isGrounded = false;
    public float groundCheckRadius;
    public Vector2 groundCheckOffset;
    public LayerMask groundLayerMask;

    [Header("CharacterSprites and Animation")]
    public Animator animator;
    public bool facingRight = true;
    public bool isAlive = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = gravityScale;

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        
        moveAction.Enable();
        jumpAction.Enable();
    }

    void Update()
    {
        bool levelCompleted = LevelScript.Larry != null ? LevelScript.Larry.levelCompleted : false;
        bool countdownActive = GameManager.Gary != null ? GameManager.Gary.isCountdownActive : false;
        
        if (transform.position.y < deathY && isAlive)
        {
            isAlive = false;
            animator.SetTrigger("Death");
            GetComponent<Collider2D>().enabled = false;
            GameManager.Gary.PlayerDeath(gameObject);
        }
        
        if (jumpAction.WasPressedThisFrame() && isGrounded && isAlive && !levelCompleted)
        {
            rb.linearVelocityY = jumpHeight;
            if (SoundManager.Steve != null)
                SoundManager.Steve.PlayJumpSound();
            animator.SetBool("Grounded", false);
            animator.SetTrigger("JumpTrigger");
            jumpInputLockTime = jumpLockDuration;
            moveDirection = 0;
        }
        else
        {
            animator.SetBool("Grounded", isGrounded);
        }

        if (jumpInputLockTime > 0)
        {
            jumpInputLockTime -= Time.deltaTime;
            moveDirection = 0;
        }
        else if (isAlive && !levelCompleted && !countdownActive)
        {
            moveDirection = moveAction.ReadValue<Vector2>().x;
        }
        else
        {
            moveDirection = 0;
        }

        animator.SetFloat("Speed", Mathf.Abs(moveDirection));

        if (moveDirection < -0.01f && facingRight)
        {
            facingRight = false;
            Vector3 currentScale = transform.localScale;
            currentScale.x *= -1f;
            transform.localScale = currentScale;
        }
        else if (moveDirection > 0.01 && !facingRight)
        {
            facingRight = true;
            Vector3 currentScale = transform.localScale;
            currentScale.x *= -1f;
            transform.localScale = currentScale;
        }

    }

    void FixedUpdate()
    {
        bool levelCompleted = LevelScript.Larry != null ? LevelScript.Larry.levelCompleted : false;
        bool countdownActive = GameManager.Gary != null ? GameManager.Gary.isCountdownActive : false;
        
        isGrounded = false;

        Vector3 groundCheckPos = transform.position + (Vector3)groundCheckOffset;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, groundCheckRadius, groundLayerMask);

        foreach (Collider2D col in colliders)
        {
            Bounds bounds = col.bounds;
            bool isPlatformEffector = col.GetComponent<PlatformEffector2D>() != null;
            bool playerAbovePlatform = transform.position.y >= bounds.max.y;
            
            if (isPlatformEffector && !playerAbovePlatform)
            {
                isGrounded = true;
                break;
            }
            
            if (playerAbovePlatform)
            {
                Vector2 closestPoint = col.ClosestPoint(groundCheckPos);
                if (closestPoint.y <= groundCheckPos.y + 0.1f)
                {
                    isGrounded = true;
                    break;
                }
            }
        }

        if (levelCompleted || countdownActive)
        {
            moveDirection = 0;
        }

        rb.linearVelocityX = moveDirection * speed;
    }

    private void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Vector3 groundCheck = groundCheckOffset;
        Gizmos.DrawWireSphere(transform.position + groundCheck, groundCheckRadius);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive)
        {
            return;
        }
        
        if (collision.gameObject.tag == "Enemy")
        {
            ContactPoint2D contact = collision.contacts[0];

            if (contact.normal.y > 0.7f)
            {
                EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();

                if (enemy != null && isAlive)
                    enemy.PlayerKilledEnemy();
            }
            else
            {
                isAlive = false;
                animator.SetTrigger("Death");
                GetComponent<Collider2D>().enabled = false;
                if (SoundManager.Steve != null)
                    SoundManager.Steve.PlayDeathSound();
                GameManager.Gary.PlayerDeath(gameObject);
            }
        }
    }
}
