using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Gary;
    public bool levelCompleted = false;
    public int score = 0;
    
    void Awake()
    {
        if (Gary && Gary != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Gary = this;
        }
    }
    
    public void PlayerDeath(GameObject player)
    {
        Destroy(player);
    }
    
    public void CompleteLevel()
    {
        levelCompleted = true;
    }
    
    public void AddScore(int points)
    {
        score += points;
    }
    
    // public void RestartLevel()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }
}
