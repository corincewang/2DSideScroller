using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject levelCompletePanel;
    public TextMeshProUGUI successMessageText;
    public Button restartButton;
    public Button nextLevelButton;
    
    [Header("Settings")]
    public string successMessage = "Congratulations!";
    
    private bool isShowing = false;
    
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
    }
    
    void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartLevel);
        }
        
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(NextLevel);
        }
    }
    
    void Update()
    {
        if (GameManager.Gary != null && GameManager.Gary.levelCompleted && !isShowing)
        {
            ShowLevelComplete();
        }
    }
    
    public void ShowLevelComplete()
    {
        if (isShowing) return;
        
        isShowing = true;
        
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
            else
            {
                RestartLevel();
            }
        }
    }
}

