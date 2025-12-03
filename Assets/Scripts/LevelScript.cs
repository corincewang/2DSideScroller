using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScript : MonoBehaviour
{
    public static LevelScript Larry;
    
    public string nextLevel;
    public float timeLimit = 120f;
    
    public bool levelCompleted;
    public float currentTime;
    public bool isTimerRunning;
    
    void Awake()
    {
        Larry = this;
    }
    
    void Start()
    {
        currentTime = 0;
        isTimerRunning = false;
    }
    
    void Update()
    {
        if (isTimerRunning && !levelCompleted && !GameManager.Gary.isGameOver)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeLimit)
            {
                isTimerRunning = false;
                GameManager.Gary.TimeUp();
            }
        }
    }
    
    public void StartTimer()
    {
        currentTime = 0;
        isTimerRunning = true;
    }
    
    public void CompleteLevel()
    {
        levelCompleted = true;
        isTimerRunning = false;
    }
    
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            CompleteLevel();
    }
    
    void OnDestroy()
    {
        if (Larry == this)
            Larry = null;
    }
}

