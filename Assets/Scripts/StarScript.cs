using UnityEngine;

public class StarScript : MonoBehaviour
{
    public int starValue = 1;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Gary.AddScore(starValue);
            SoundManager.Steve.PlayStarSound();
            Destroy(gameObject);
        }
    }
}

