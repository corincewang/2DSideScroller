using UnityEngine;

public class BombScript : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.down * speed;
        
        Destroy(this.gameObject, lifeTime);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            GameManager.Gary.PlayerDeath();
            Destroy(this.gameObject);
        }
    }
}

