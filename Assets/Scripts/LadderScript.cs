using UnityEngine;
using UnityEngine.InputSystem;

public class LadderScript : MonoBehaviour
{
    public float speed = 6f;
    
    private float vertical;
    private bool isLadder;
    private bool isClimbing;
    private Rigidbody2D rb;
    
    // used guidance from youtube: https://www.youtube.com/watch?v=yyg0yV2roPk by bendux
    void Update()
    {
        vertical = InputSystem.actions.FindAction("Move").ReadValue<Vector2>().y;
        
        if (isLadder && Mathf.Abs(vertical) > 0f)
            isClimbing = true;
    }
    
    void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 4f;
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isLadder = true;
            rb = collision.GetComponent<Rigidbody2D>();
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}
