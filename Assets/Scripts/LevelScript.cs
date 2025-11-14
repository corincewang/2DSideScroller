using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScript : MonoBehaviour
{
    public static LevelScript Larry;
    
    public bool levelCompleted = false;
    public int currentLevel = 1;
    public int totalLevels = 1;
    
    [Header("Timer System")]
    public float levelTimeLimit = 120f;
    public float currentTime = 0f;
    public bool isTimerRunning = false;
    
    void Awake()
    {
        if (Larry && Larry != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Larry = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        currentTime = 0f;
        isTimerRunning = false;
    }
    
    void Update()
    {
        if (isTimerRunning && !levelCompleted && (GameManager.Gary == null || !GameManager.Gary.isGameOver))
        {
            currentTime += Time.deltaTime;
            
            if (currentTime >= levelTimeLimit)
            {
                TimeUp();
            }
        }
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        currentTime = 0f;
        isTimerRunning = false;
    }
    
    public void CompleteLevel()
    {
        levelCompleted = true;
        StopTimer();
    }
    
    public void ResetLevelState()
    {
        levelCompleted = false;
    }
    
    public void StartTimer()
    {
        currentTime = 0f;
        isTimerRunning = true;
    }
    
    void StopTimer()
    {
        isTimerRunning = false;
    }
    
    void TimeUp()
    {
        StopTimer();
        if (GameManager.Gary != null)
            GameManager.Gary.TimeUp();
    }
    
    public void LoadNextLevel()
    {
        ResetLevelState();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            currentLevel = nextSceneIndex;
        }
        else
        {
            SceneManager.LoadScene(0);
            currentLevel = 0;
        }
    }
    
    public void RestartLevel()
    {
        ResetLevelState();
        StartTimer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CompleteLevel();
        }
    }
}

