using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// based upon the 2DCharacterController from Sharp Coder Blog

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class CharacterController2D : MonoBehaviour
{
    public float speed = 1f;
    public float jumpHeight = 2f;
    public float gravityScale = 1f;

    private Rigidbody2D rb;
    private InputAction moveAction, jumpAction;
    private float moveDirection;

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
        // define rigidbody
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = gravityScale;

        // define actions
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            moveDirection = moveAction.ReadValue<Vector2>().x;
        }
        else
        {
            moveDirection = 0;
        }


        animator.SetFloat("Speed", Mathf.Abs(moveDirection));

        if (jumpAction.WasPressedThisFrame() && isGrounded && isAlive)
        {
            rb.linearVelocityY = jumpHeight;
            SoundManager.Steve.PlayJumpSound();
            animator.SetBool("Grounded", false);
            animator.SetTrigger("JumpTrigger");
        }
        else
        {
            animator.SetBool("Grounded", isGrounded);
        }



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
        isGrounded = false;

        Vector3 groundCheck = groundCheckOffset;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + groundCheck, groundCheckRadius, groundLayerMask);

        if (colliders.Length > 0)
        {
            isGrounded = true;
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
}

// OnCollisionEnter
// if (!isAlive){return};

// animator.setTrigger("Death");
// isAlive = false;
