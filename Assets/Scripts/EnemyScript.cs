using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float enemySpeed;
    public bool isAlive = true;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public bool facingLeft = true;
    private GameObject playerToDestroy;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.Steve.PlayEnemyHitSound();
            Invoke(nameof(PlayDeathSoundAndDestroy), 0.5f);
            playerToDestroy = collision.gameObject;
        }
    }
    
    public void PlayerKilledEnemy()
    {
        if (isAlive)
        {
            isAlive = false;
            animator.SetTrigger("EnemyDeath");
            rb.bodyType = RigidbodyType2D.Kinematic;

            gameObject.layer = LayerMask.NameToLayer("NotForPlayer");
            Destroy(gameObject, 1f);
        }

    }
    
    void PlayDeathSoundAndDestroy()
    {
        SoundManager.Steve.PlayDeathSound();
        GameManager.Gary.PlayerDeath(playerToDestroy);
    }
}
