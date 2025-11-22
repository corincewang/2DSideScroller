using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float enemySpeed;
    public bool isAlive = true;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public bool facingLeft = true;

    private Animator animator;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (facingLeft == sprite.flipX)
        {
            sprite.flipX = !facingLeft;
        }
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            rb.linearVelocityX = facingLeft ? -enemySpeed : enemySpeed;
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TurnAround")
        {
            facingLeft = !facingLeft;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive) return;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterController2D playerController = collision.gameObject.GetComponent<CharacterController2D>();
            if (playerController && !playerController.isAlive) return;
            
            ContactPoint2D contact = collision.contacts[0];
            
            if (contact.normal.y < -0.5f)
                return;
        }
    }
    
    public void PlayerKilledEnemy()
    {
        if (isAlive)
        {
            isAlive = false;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            
            GetComponent<Collider2D>().enabled = false;
            
            animator.SetTrigger("EnemyDeath");
            if (SoundManager.Steve != null)
                SoundManager.Steve.PlayEnemyHitSound();
            
            Destroy(gameObject, 0.6f);
        }
    }
}
