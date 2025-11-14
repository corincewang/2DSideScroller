using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Gary;
    public int score = 0;
    
    [Header("Player Respawn")]
    public GameObject playerPrefab;
    public float respawnDelay = 1f;
    
    [Header("Lives System")]
    public int maxLives = 3;
    public int currentLives = 3;
    public bool isGameOver = false;
    
    private GameObject currentPlayer;
    private Vector3? checkpointPosition = null;
    private UIManager uiManager;
    
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
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
        uiManager = FindObjectOfType<UIManager>();
        ResetLives();
        ResetCheckpoint();
        ResetAllCheckpoints();
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
        uiManager = FindObjectOfType<UIManager>();
        ResetLives();
        ResetCheckpoint();
        ResetAllCheckpoints();
    }
    
    void ResetAllCheckpoints()
    {
        LevelCheckpoint[] checkpoints = FindObjectsOfType<LevelCheckpoint>();
        foreach (LevelCheckpoint checkpoint in checkpoints)
        {
            checkpoint.ResetCheckpoint();
        }
    }
    
    public void PlayerDeath(GameObject player)
    {
        if (isGameOver) return;
        
        currentPlayer = player;
        player.SetActive(false);
        
        currentLives--;
        
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();
        
        if (currentLives > 0)
        {
            uiManager.ShowOops();
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
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();
        
        uiManager.ShowGameOver();
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
            if (checkpointPosition.HasValue)
            {
                RespawnPlayerAtPosition(playerToRespawn, checkpointPosition.Value);
            }
            else
            {
                LevelCheckpoint.currentStartPoint.RespawnPlayer(playerToRespawn);
            }
            currentPlayer = playerToRespawn;
            
            OnPlayerRespawn?.Invoke();
        }
        else
        {
            RestartLevel();
        }
    }
    
    public void AddScore(int points)
    {
        score += points;
    }
    
    public void TimeUp()
    {
        isGameOver = true;
        
        if (currentPlayer != null)
        {
            CharacterController2D controller = currentPlayer.GetComponent<CharacterController2D>();
            if (controller != null && controller.isAlive)
            {
                controller.isAlive = false;
                controller.GetComponent<Collider2D>().enabled = false;
                controller.animator.SetTrigger("Death");
            }
        }
        
        Invoke(nameof(ShowGameOver), respawnDelay);
    }
    
    public void ResetLives()
    {
        currentLives = maxLives;
        isGameOver = false;
        if (LevelScript.Larry != null)
            LevelScript.Larry.ResetLevelState();
    }
    
    public void RestartLevel()
    {
        isGameOver = false;
        ResetLives();
        ResetCheckpoint();
        ResetAllCheckpoints();
        score = 0;
        if (LevelScript.Larry != null)
            LevelScript.Larry.RestartLevel();
    }
    
    public void SetCheckpointPosition(Vector3 position)
    {
        checkpointPosition = position;
    }
    
    void ResetCheckpoint()
    {
        checkpointPosition = null;
    }
    
    void RespawnPlayerAtPosition(GameObject player, Vector3 position)
    {
        player.transform.position = position;
        
        CharacterController2D controller = player.GetComponent<CharacterController2D>();
        controller.isAlive = true;
        controller.GetComponent<Collider2D>().enabled = true;
        
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
    }
}
