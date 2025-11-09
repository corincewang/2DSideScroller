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

    }

    void Update()
    {
        bool levelCompleted = GameManager.Gary != null && GameManager.Gary.levelCompleted;
        
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
        else if (isAlive && !levelCompleted)
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
        bool levelCompleted = GameManager.Gary != null && GameManager.Gary.levelCompleted;
        
        isGrounded = false;

        Vector3 groundCheck = groundCheckOffset;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + groundCheck, groundCheckRadius, groundLayerMask);

        if (colliders.Length > 0)
        {
            isGrounded = true;
        }

        if (levelCompleted)
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

                if (enemy && isAlive)
                {
                    enemy.PlayerKilledEnemy();
                }
                else
                {
                    return;
                }
            }
            else
            {
                isAlive = false;
                animator.SetTrigger("Death");
                GetComponent<Collider2D>().enabled = false;
                GameManager.Gary.PlayerDeath(gameObject);
            }
        }
    }
}


