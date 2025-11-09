using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Gary;
    public bool levelCompleted = false;
    public int score = 0;
    
    [Header("Level Management")]
    public int currentLevel = 1;
    public int totalLevels = 1;
    
    [Header("Player Respawn")]
    public GameObject playerPrefab;
    public float respawnDelay = 1f;
    
    [Header("Lives System")]
    public int maxLives = 3;
    public int currentLives = 3;
    public bool isGameOver = false;
    
    private GameObject currentPlayer;
    
    void Awake()
    {
        if (Gary && Gary != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Gary = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
        ResetLives();
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
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
        ResetLives();
    }
    
    public void PlayerDeath(GameObject player)
    {
        if (isGameOver) return;
        
        currentPlayer = player;
        if (player != null)
        {
            player.SetActive(false);
        }
        
        currentLives--;
        
        if (currentLives > 0)
        {
            Invoke(nameof(RespawnPlayer), respawnDelay);
        }
        else
        {
            isGameOver = true;
            Invoke(nameof(ShowGameOver), respawnDelay);
        }
    }
    
    void ShowGameOver()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
    }
    
    public System.Action OnPlayerRespawn;
    
    void RespawnPlayer()
    {
        GameObject playerToRespawn = null;
        
        if (currentPlayer != null && !currentPlayer.activeSelf)
        {
            playerToRespawn = currentPlayer;
            playerToRespawn.SetActive(true);
        }
        else if (currentPlayer == null)
        {
            playerToRespawn = GameObject.FindGameObjectWithTag("Player");
            
            if (playerToRespawn == null && playerPrefab != null)
            {
                playerToRespawn = Instantiate(playerPrefab);
            }
        }
        else
        {
            playerToRespawn = currentPlayer;
        }
        
        if (playerToRespawn != null)
        {
            LevelCheckpoint.currentStartPoint.RespawnPlayer(playerToRespawn);
            currentPlayer = playerToRespawn;
            
            OnPlayerRespawn?.Invoke();
        }
        else
        {
            RestartLevel();
        }
    }
    
    public void CompleteLevel()
    {
        levelCompleted = true;
    }
    
    public void AddScore(int points)
    {
        score += points;
    }
    
    public void ResetLevelState()
    {
        levelCompleted = false;
        isGameOver = false;
    }
    
    public void ResetLives()
    {
        currentLives = maxLives;
        isGameOver = false;
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
        ResetLives();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
