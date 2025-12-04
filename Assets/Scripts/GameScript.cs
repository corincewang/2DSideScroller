using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private int score;
    public int livesRemaining = 3;
    
    public static GameManager Gary;
    
    public bool isGameOver;
    public bool isCountdownActive;
    public float respawnDelay = 1f;
    public Vector3 spawnPoint;
    public float checkpointX = 60f;
    public float timeLimit = 120f;
    
    public TextMeshProUGUI messageOverlay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI livesDisplay;
    public TextMeshProUGUI timerDisplay;
    
    private float currentTime;
    private bool isTimerRunning;
    
    public System.Action OnPlayerRespawn;
    
    private GameObject player;
    
    void Awake()
    {
        if (Gary)
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
        LevelStarted();
    }
    
    public void LevelStarted()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = player.transform.position;
        livesRemaining = 3;
        isGameOver = false;
        currentTime = 0;
        isTimerRunning = false;
        StartCoroutine(GetReady());
    }
    
    public IEnumerator GetReady()
    {
        isCountdownActive = true;
        yield return new WaitForSeconds(0.1f);
        
        if (messageOverlay)
        {
            string[] countdownMessages = { "3", "2", "1", "GO!" };
            
            for (int i = 0; i < countdownMessages.Length; i++)
            {
                messageOverlay.text = countdownMessages[i];
                messageOverlay.enabled = true;
                yield return new WaitForSeconds(0.5f);
                messageOverlay.enabled = false;
                yield return new WaitForSeconds(0.5f);
            }
            
            messageOverlay.enabled = false;
        }
        
        isCountdownActive = false;
        isTimerRunning = true;
    }
    
    void Update()
    {
        if (player != null && player.transform.position.x >= checkpointX)
            spawnPoint = new Vector3(checkpointX, spawnPoint.y, 0);
            
        UpdateScoreDisplay();
        UpdateLivesDisplay();
        UpdateTimer();
    }
    
    private void UpdateScoreDisplay()
    {
        if (scoreDisplay)
            scoreDisplay.text = "Score: " + score;
    }
    
    private void UpdateLivesDisplay()
    {
        if (livesDisplay)
            livesDisplay.text = "Lives: " + livesRemaining;
    }
    
    private void UpdateTimer()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeLimit)
            {
                isTimerRunning = false;
                TimeUp();
            }
            
            if (timerDisplay)
            {
                float time = Mathf.Max(0, timeLimit - currentTime);
                timerDisplay.text = "Time: " + Mathf.FloorToInt(time) + "s";
            }
        }
    }
    
    public void PlayerDeath(GameObject p)
    {
        if (isGameOver) return;
        
        player = p;
        p.SetActive(false);
        livesRemaining--;
        
        if (livesRemaining > 0)
            StartCoroutine(OopsState());
        else
        {
            isGameOver = true;
            StartCoroutine(GameOverLoseState());
        }
    }
    
    IEnumerator OopsState()
    {
        if (messageOverlay)
        {
            messageOverlay.enabled = true;
            messageOverlay.text = "Oops!";
        }
        yield return new WaitForSeconds(respawnDelay);
        
        RespawnPlayer();
        if (messageOverlay) messageOverlay.enabled = false;
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
    
    IEnumerator GameOverLoseState()
    {
        isGameOver = true;
        
        if (messageOverlay)
        {
            messageOverlay.enabled = true;
            messageOverlay.text = "Game Over! \nScore: " + score;
        }
        
        yield return new WaitForSeconds(3f);
        if (messageOverlay) messageOverlay.enabled = false;
        
        SceneManager.LoadScene("MenuScene");
    }
    
    public void TimeUp()
    {
        isGameOver = true;
        CharacterController2D controller = player.GetComponent<CharacterController2D>();
        controller.isAlive = false;
        controller.GetComponent<Collider2D>().enabled = false;
        controller.animator.SetTrigger("Death");
        StartCoroutine(GameOverLoseState());
    }
    
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }
}
