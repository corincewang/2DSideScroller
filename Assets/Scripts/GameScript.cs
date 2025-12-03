using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Gary;
    
    public int score;
    public int maxLives = 3;
    public int currentLives = 3;
    public bool isGameOver;
    public float respawnDelay = 1f;
    public Vector3 spawnPoint;
    public float checkpointX = 60f;
    
    public System.Action OnPlayerRespawn;
    
    private GameObject player;
    
    void Awake()
    {
        if (Gary && Gary != this)
            Destroy(gameObject);
        else
        {
            Gary = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = player.transform.position;
        currentLives = maxLives;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = player.transform.position;
        currentLives = maxLives;
        isGameOver = false;
    }
    
    void Update()
    {
        if (player != null && player.transform.position.x >= checkpointX)
            spawnPoint = new Vector3(checkpointX, spawnPoint.y, 0);
    }
    
    public void PlayerDeath(GameObject p)
    {
        if (isGameOver) return;
        
        player = p;
        p.SetActive(false);
        currentLives--;
        
        if (currentLives > 0)
        {
            FindObjectOfType<UIManager>().ShowOops();
            Invoke(nameof(RespawnPlayer), respawnDelay);
        }
        else
        {
            isGameOver = true;
            Invoke(nameof(ShowGameOver), respawnDelay);
        }
    }
    
    void RespawnPlayer()
    {
        player.SetActive(true);
        player.transform.position = spawnPoint;
        
        CharacterController2D controller = player.GetComponent<CharacterController2D>();
        controller.isAlive = true;
        controller.GetComponent<Collider2D>().enabled = true;
        
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        
        OnPlayerRespawn?.Invoke();
    }
    
    void ShowGameOver()
    {
        FindObjectOfType<UIManager>().ShowGameOver();
    }
    
    public void TimeUp()
    {
        isGameOver = true;
        CharacterController2D controller = player.GetComponent<CharacterController2D>();
        controller.isAlive = false;
        controller.GetComponent<Collider2D>().enabled = false;
        controller.animator.SetTrigger("Death");
        Invoke(nameof(ShowGameOver), respawnDelay);
    }
    
    public void AddScore(int points)
    {
        score += points;
    }
    
    public void RestartLevel()
    {
        isGameOver = false;
        currentLives = maxLives;
        score = 0;
        if (LevelScript.Larry != null)
            LevelScript.Larry.RestartLevel();
    }
}
