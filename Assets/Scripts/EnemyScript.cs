using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float enemySpeed;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public bool facingLeft = true;
    private GameObject playerToDestroy;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
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
        rb.linearVelocityX = facingLeft ? -enemySpeed : enemySpeed;
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
    

    
    void PlayDeathSoundAndDestroy()
    {
        SoundManager.Steve.PlayDeathSound();
        GameManager.Gary.PlayerDeath(playerToDestroy);
    }
}
