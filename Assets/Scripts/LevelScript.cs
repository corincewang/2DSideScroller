using UnityEngine;

public class LevelScript : MonoBehaviour
{
    // will do level in the future
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Gary.CompleteLevel();
        }
    }
}

