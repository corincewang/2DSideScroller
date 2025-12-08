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
    public float checkpointX;
    public float timeLimit = 120f;
    
    public TextMeshProUGUI messageOverlay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI livesDisplay;
    public TextMeshProUGUI timerDisplay;
    
    private float currentTime;
    private bool isTimerRunning;
    
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
        ResetGameState();
    }
    
    public void LevelStarted()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = player.transform.position;
        livesRemaining = 3;
        isGameOver = false;
        currentTime = 0;
        isTimerRunning = false;
        
        if (scoreDisplay) scoreDisplay.enabled = true;
        if (livesDisplay) livesDisplay.enabled = true;
        if (timerDisplay) timerDisplay.enabled = true;
        
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        timeLimit = sceneIndex == 4 ? 30f : 120f;
        
        if (SoundManager.Steve)
            SoundManager.Steve.PlayBackgroundMusicForCurrentScene();
        
        StartCoroutine(GetReady());
    }
    
    public IEnumerator GetReady()
    {
        isCountdownActive = true;
        
        if (messageOverlay)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (sceneIndex == 4)
            {
                messageOverlay.text = "Survive 30s!";
                messageOverlay.enabled = true;
                yield return new WaitForSeconds(2f);
                messageOverlay.enabled = false;
                yield return new WaitForSeconds(0.5f);
            }
            
            string[] countdownMessages = { "3", "2", "1", "GO!" };
            for (int i = 0; i < 4; i++)
            {
                messageOverlay.text = countdownMessages[i];
                messageOverlay.enabled = true;
                yield return new WaitForSeconds(0.5f);
                messageOverlay.enabled = false;
                yield return new WaitForSeconds(0.5f);
            }
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
    
    void UpdateScoreDisplay()
    {
        if (scoreDisplay)
        {
            scoreDisplay.text = "Score: " + score;
        }
    }
    
    void UpdateLivesDisplay()
    {
        if (livesDisplay)
        {
            livesDisplay.text = "Lives: " + livesRemaining;
        }
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
    
    public void PlayerDeath()
    {
        if (isGameOver) return;
        
        player.SetActive(false);
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
        player.GetComponent<Collider2D>().enabled = true;
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        
        FerryPlatform[] platforms = FindObjectsOfType<FerryPlatform>();
        foreach (FerryPlatform platform in platforms)
        {
            platform.ResetPlatform();
        }
    }
    
    IEnumerator GameOverLoseState()
    {
        isGameOver = true;
        isTimerRunning = false;
        
        if (messageOverlay)
        {
            messageOverlay.enabled = true;
            messageOverlay.text = "Game Over! \nScore: " + score;
        }
        
        if (SoundManager.Steve)
        {
            SoundManager.Steve.GetComponent<AudioSource>().Stop();
        }
        
        yield return new WaitForSeconds(5f);
        if (messageOverlay) messageOverlay.enabled = false;
        
        SceneManager.LoadScene("MenuScene");
    }
    
    public void TimeUp()
    {
        isGameOver = true;
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        //if currently in boss level and time up, player win
        if (sceneIndex == 4)
        {
            StartCoroutine(BossWinState());
        }
        else
        {
            CharacterController2D controller = player.GetComponent<CharacterController2D>();
            controller.isAlive = false;
            controller.GetComponent<Collider2D>().enabled = false;
            controller.animator.SetTrigger("Death");
            StartCoroutine(GameOverLoseState());
        }
    }
    
    IEnumerator BossWinState()
    {
        isGameOver = true;
        isTimerRunning = false;
        
        if (messageOverlay)
        {
            messageOverlay.enabled = true;
            messageOverlay.text = "You Win! \nScore: " + score;
        }
        
        if (SoundManager.Steve)
        {
            SoundManager.Steve.GetComponent<AudioSource>().Stop();
            SoundManager.Steve.PlayLevelCompleteSound();
        }
        
        yield return new WaitForSeconds(4f);
        
        if (messageOverlay) messageOverlay.enabled = false;
        SceneManager.LoadScene("MenuScene");
    }
    
    public void AddScore(int points)
    {
        score += points;
    }
    
    public void ResetGameState()
    {
        //reset all game state when start from menu
        score = 0;
        livesRemaining = 3;
        isGameOver = false;
        isCountdownActive = false;
        currentTime = 0;
        isTimerRunning = false;
        player = null;
    }

}