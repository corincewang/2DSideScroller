using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Info")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;
    
    [Header("Level Complete UI")]
    public GameObject levelCompletePanel;
    public TextMeshProUGUI successMessageText;
    public Button nextLevelButton;
    
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverMessageText;
    public Button gameOverRestartButton;
    
    [Header("Ready State UI")]
    public GameObject readyPanel;
    public TextMeshProUGUI readyMessageText;
    public float readyDisplayDuration = 1f;
    public float countdownInterval = 1f;
    
    [Header("Oops State UI")]
    public GameObject oopsPanel;
    public TextMeshProUGUI oopsMessageText;
    public float oopsDisplayDuration = 2f;
    
    [Header("Settings")]
    public string readyMessage = "Ready!";
    public string oopsMessage = "Oops!";
    public string successMessage = "Congratulations!";
    public string gameOverMessage = "Game Over";
    public string nextLevelButtonText = "Next Level";
    public string restartButtonText = "Restart";
    
    private bool isLevelCompleteShowing = false;
    private bool isGameOverShowing = false;
    private bool isReadyShowing = false;
    private bool isOopsShowing = false;
    
    public bool isCountdownActive = true;
    
    void Start()
    {
        levelCompletePanel.SetActive(false);
        successMessageText.gameObject.SetActive(false);
        gameOverPanel.SetActive(false);
        gameOverMessageText.gameObject.SetActive(false);
        oopsPanel.SetActive(false);
        oopsMessageText.gameObject.SetActive(false);
        readyPanel.SetActive(false);
        readyMessageText.gameObject.SetActive(false);
        
        nextLevelButton.onClick.AddListener(NextLevel);
        SetButtonText(nextLevelButton, nextLevelButtonText);
        gameOverRestartButton.onClick.AddListener(RestartGame);
        SetButtonText(gameOverRestartButton, restartButtonText);
        
        Invoke(nameof(ShowReady), 0.1f);
    }
    
    
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        levelCompletePanel.SetActive(false);
        successMessageText.gameObject.SetActive(false);
        gameOverPanel.SetActive(false);
        gameOverMessageText.gameObject.SetActive(false);
        oopsPanel.SetActive(false);
        oopsMessageText.gameObject.SetActive(false);
        readyPanel.SetActive(false);
        readyMessageText.gameObject.SetActive(false);
        
        isLevelCompleteShowing = false;
        isGameOverShowing = false;
        isReadyShowing = false;
        isOopsShowing = false;
        isCountdownActive = true;
        Invoke(nameof(ShowReady), 0.1f);
    }
    
    void SetButtonText(Button button, string text)
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = text;
    }
    
    void Update()
    {
        if (GameManager.Gary != null)
        {
            scoreText.text = "Score: " + GameManager.Gary.score;
            livesText.text = "Lives: " + GameManager.Gary.currentLives;
            
            if (LevelScript.Larry != null && LevelScript.Larry.isTimerRunning)
            {
                float remainingTime = LevelScript.Larry.timeLimit - LevelScript.Larry.currentTime;
                if (remainingTime < 0) remainingTime = 0;
                int seconds = Mathf.FloorToInt(remainingTime);
                timerText.text = "Time: " + seconds + "s";
            }
            else
            {
                timerText.text = "Time: 120s";
            }
            
            if (LevelScript.Larry != null && LevelScript.Larry.levelCompleted && !isLevelCompleteShowing)
            {
                ShowLevelComplete();
            }
        }
    }
    
    public void ShowLevelComplete()
    {
        if (isLevelCompleteShowing) return;
        
        isLevelCompleteShowing = true;
        
        levelCompletePanel.SetActive(true);
        successMessageText.text = successMessage;
        successMessageText.gameObject.SetActive(true);
        
        if (SoundManager.Steve != null)
        {
            AudioSource bgMusic = SoundManager.Steve.GetComponent<AudioSource>();
            if (bgMusic != null && bgMusic.isPlaying)
                bgMusic.Stop();
            
            SoundManager.Steve.PlayLevelCompleteSound();
        }
    }
    
    public void ShowReady()
    {
        if (isReadyShowing) return;
        
        isReadyShowing = true;
        isCountdownActive = true;
        
        readyPanel.SetActive(true);
        readyMessageText.text = readyMessage;
        readyMessageText.gameObject.SetActive(true);
        
        Invoke(nameof(ShowCountdown3), readyDisplayDuration);
    }
    
    void ShowCountdown3()
    {
        readyMessageText.text = "3";
        Invoke(nameof(ShowCountdown2), countdownInterval);
    }
    
    void ShowCountdown2()
    {
        readyMessageText.text = "2";
        Invoke(nameof(ShowCountdown1), countdownInterval);
    }
    
    void ShowCountdown1()
    {
        readyMessageText.text = "1";
        Invoke(nameof(HideReady), countdownInterval);
    }
    
    void HideReady()
    {
        readyPanel.SetActive(false);
        readyMessageText.gameObject.SetActive(false);
        
        isReadyShowing = false;
        isCountdownActive = false;
        
        if (LevelScript.Larry != null)
            LevelScript.Larry.StartTimer();
    }
    
    public void ShowOops()
    {
        CancelInvoke(nameof(HideOops));
        
        isOopsShowing = true;
        
        oopsPanel.SetActive(true);
        oopsMessageText.text = oopsMessage;
        oopsMessageText.gameObject.SetActive(true);
        
        Invoke(nameof(HideOops), oopsDisplayDuration);
    }
    
    void HideOops()
    {
        oopsPanel.SetActive(false);
        oopsMessageText.gameObject.SetActive(false);
        
        isOopsShowing = false;
    }
    
    public void ShowGameOver()
    {
        if (isGameOverShowing) return;
        
        isGameOverShowing = true;
        
        gameOverPanel.SetActive(true);
        gameOverMessageText.text = gameOverMessage;
        gameOverMessageText.gameObject.SetActive(true);
        
        if (SoundManager.Steve != null)
        {
            AudioSource bgMusic = SoundManager.Steve.GetComponent<AudioSource>();
            if (bgMusic != null && bgMusic.isPlaying)
                bgMusic.Stop();
        }
    }
    
    public void RestartLevel()
    {
        GameManager.Gary.RestartLevel();
    }
    
    public void NextLevel()
    {
        if (LevelScript.Larry != null)
            LevelScript.Larry.LoadNextLevel();
    }
    
    public void RestartGame()
    {
        GameManager.Gary.RestartLevel();
    }
}

