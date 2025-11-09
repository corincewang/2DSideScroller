using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Info")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    
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
    
    void Awake()
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }
        
        if (successMessageText != null)
        {
            successMessageText.gameObject.SetActive(false);
        }
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        if (gameOverMessageText != null)
        {
            gameOverMessageText.gameObject.SetActive(false);
        }
        
        if (readyPanel != null)
        {
            readyPanel.SetActive(false);
        }
        
        if (readyMessageText != null)
        {
            readyMessageText.gameObject.SetActive(false);
        }
        
        if (oopsPanel != null)
        {
            oopsPanel.SetActive(false);
        }
        
        if (oopsMessageText != null)
        {
            oopsMessageText.gameObject.SetActive(false);
        }
    }
    
    void Start()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(NextLevel);
            SetButtonText(nextLevelButton, nextLevelButtonText);
        }
        
        if (gameOverRestartButton != null)
        {
            gameOverRestartButton.onClick.AddListener(RestartGame);
            SetButtonText(gameOverRestartButton, restartButtonText);
        }
        
        Invoke(nameof(ShowReady), 0.1f);
    }
    
    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        isLevelCompleteShowing = false;
        isGameOverShowing = false;
        isReadyShowing = false;
        isOopsShowing = false;
        isCountdownActive = true;
        Invoke(nameof(ShowReady), 0.1f);
    }
    
    void SetButtonText(Button button, string text)
    {
        if (button == null) return;
        
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = text;
        }
    }
    
    void Update()
    {
        if (GameManager.Gary != null)
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + GameManager.Gary.score;
            }
            
            if (livesText != null)
            {
                livesText.text = "Lives: " + GameManager.Gary.currentLives;
            }
            
            if (GameManager.Gary.levelCompleted && !isLevelCompleteShowing)
            {
                ShowLevelComplete();
            }
        }
    }
    
    public void ShowLevelComplete()
    {
        if (isLevelCompleteShowing) return;
        
        isLevelCompleteShowing = true;
        
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }
        
        if (successMessageText != null)
        {
            successMessageText.text = successMessage;
            successMessageText.gameObject.SetActive(true);
        }
        
        if (SoundManager.Steve != null)
        {
            AudioSource bgMusic = SoundManager.Steve.GetComponent<AudioSource>();
            if (bgMusic != null && bgMusic.isPlaying)
            {
                bgMusic.Stop();
            }
            
            SoundManager.Steve.PlayLevelCompleteSound();
        }
    }
    
    public void ShowReady()
    {
        if (isReadyShowing) return;
        
        isReadyShowing = true;
        isCountdownActive = true;
        
        if (readyPanel != null)
        {
            readyPanel.SetActive(true);
        }
        
        if (readyMessageText != null)
        {
            readyMessageText.text = readyMessage;
            readyMessageText.gameObject.SetActive(true);
        }
        
        Invoke(nameof(ShowCountdown3), readyDisplayDuration);
    }
    
    void ShowCountdown3()
    {
        if (readyMessageText != null)
        {
            readyMessageText.text = "3";
        }
        Invoke(nameof(ShowCountdown2), countdownInterval);
    }
    
    void ShowCountdown2()
    {
        if (readyMessageText != null)
        {
            readyMessageText.text = "2";
        }
        Invoke(nameof(ShowCountdown1), countdownInterval);
    }
    
    void ShowCountdown1()
    {
        if (readyMessageText != null)
        {
            readyMessageText.text = "1";
        }
        Invoke(nameof(HideReady), countdownInterval);
    }
    
    void HideReady()
    {
        if (readyPanel != null)
        {
            readyPanel.SetActive(false);
        }
        
        if (readyMessageText != null)
        {
            readyMessageText.gameObject.SetActive(false);
        }
        
        isReadyShowing = false;
        isCountdownActive = false;
    }
    
    public void ShowOops()
    {
        CancelInvoke(nameof(HideOops));
        
        isOopsShowing = true;
        
        if (oopsPanel != null)
        {
            oopsPanel.SetActive(true);
        }
        
        if (oopsMessageText != null)
        {
            oopsMessageText.text = oopsMessage;
            oopsMessageText.gameObject.SetActive(true);
        }
        
        Invoke(nameof(HideOops), oopsDisplayDuration);
    }
    
    void HideOops()
    {
        if (oopsPanel != null)
        {
            oopsPanel.SetActive(false);
        }
        
        if (oopsMessageText != null)
        {
            oopsMessageText.gameObject.SetActive(false);
        }
        
        isOopsShowing = false;
    }
    
    public void ShowGameOver()
    {
        if (isGameOverShowing) return;
        
        isGameOverShowing = true;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        if (gameOverMessageText != null)
        {
            gameOverMessageText.text = gameOverMessage;
            gameOverMessageText.gameObject.SetActive(true);
        }
        
        if (SoundManager.Steve != null)
        {
            AudioSource bgMusic = SoundManager.Steve.GetComponent<AudioSource>();
            if (bgMusic != null && bgMusic.isPlaying)
            {
                bgMusic.Stop();
            }
        }
    }
    
    public void RestartLevel()
    {
        if (GameManager.Gary != null)
        {
            GameManager.Gary.RestartLevel();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            );
        }
    }
    
    public void NextLevel()
    {
        if (GameManager.Gary != null)
        {
            GameManager.Gary.LoadNextLevel();
        }
        else
        {
            int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
            }
        }
    }
    
    public void RestartGame()
    {
        isGameOverShowing = false;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        if (gameOverMessageText != null)
        {
            gameOverMessageText.gameObject.SetActive(false);
        }
        
        if (GameManager.Gary != null)
        {
            GameManager.Gary.RestartLevel();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            );
        }
    }
}

