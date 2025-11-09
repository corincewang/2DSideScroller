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
    
    [Header("Settings")]
    public string successMessage = "Congratulations!";
    public string gameOverMessage = "Game Over";
    public string nextLevelButtonText = "Next Level";
    public string restartButtonText = "Restart";
    
    private bool isLevelCompleteShowing = false;
    private bool isGameOverShowing = false;
    
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

