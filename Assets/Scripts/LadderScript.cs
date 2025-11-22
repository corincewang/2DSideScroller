using UnityEngine;
using UnityEngine.InputSystem;

public class LadderScript : MonoBehaviour
{
    [Header("Ladder Settings")]
    public float climbSpeed = 3f;
    public bool allowJumpOffLadder = true;
    
    private bool isPlayerOnLadder = false;
    private bool isClimbing = false;
    private GameObject playerOnLadder = null;
    private Rigidbody2D playerRb = null;
    private CharacterController2D playerController = null;
    private float originalGravityScale = 1f;
    private InputAction moveAction, jumpAction;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnLadder = true;
            isClimbing = false;
            playerOnLadder = collision.gameObject;
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerController = collision.gameObject.GetComponent<CharacterController2D>();
            
            if (playerRb != null)
            {
                originalGravityScale = playerRb.gravityScale;
            }
            
            if (moveAction == null)
            {
                moveAction = InputSystem.actions.FindAction("Move");
            }
            if (jumpAction == null)
            {
                jumpAction = InputSystem.actions.FindAction("Jump");
            }
        }
    }
    
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerRb != null)
        {
            float verticalInput = 0f;
            if (moveAction != null)
            {
                verticalInput = moveAction.ReadValue<Vector2>().y;
            }
            
            if (allowJumpOffLadder && jumpAction != null && jumpAction.WasPressedThisFrame() && isClimbing)
            {
                ExitClimbingMode();
                return;
            }
            
            if (Mathf.Abs(verticalInput) > 0.1f)
            {
                isClimbing = true;
                playerRb.gravityScale = 0f;
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, verticalInput * climbSpeed);
            }
            else if (isClimbing)
            {
                playerRb.gravityScale = 0f;
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);
            }
            else
            {
                if (playerRb.gravityScale != originalGravityScale)
                {
                    playerRb.gravityScale = originalGravityScale;
                }
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ExitClimbingMode();
        }
    }
    
    void ExitClimbingMode()
    {
        isPlayerOnLadder = false;
        isClimbing = false;
        
        if (playerRb != null)
        {
            playerRb.gravityScale = originalGravityScale;
        }
        
        playerOnLadder = null;
        playerRb = null;
        playerController = null;
    }
}
