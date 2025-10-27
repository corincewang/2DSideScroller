using UnityEngine;
// ToDO: next week implement level
public class LevelScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Gary.CompleteLevel();
        }
    }
}

