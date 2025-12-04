using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LevelScript : MonoBehaviour
{
    public static LevelScript Larry;
    
    public string nextLevel;
    
    public bool levelCompleted;
    
    void Awake()
    {
        Larry = this;
    }
    
    void Start()
    {
        if (GameManager.Gary != null)
            GameManager.Gary.LevelStarted();
    }
    
    public void CompleteLevel()
    {
        levelCompleted = true;
        StartCoroutine(LevelWin());
    }
    
    IEnumerator LevelWin()
    {
        if (GameManager.Gary && GameManager.Gary.messageOverlay)
        {
            GameManager.Gary.messageOverlay.enabled = true;
            GameManager.Gary.messageOverlay.text = "Level Cleared!!";
        }
        
        if (SoundManager.Steve)
        {
            SoundManager.Steve.GetComponent<AudioSource>().Stop();
            SoundManager.Steve.PlayLevelCompleteSound();
        }
        
        yield return new WaitForSeconds(4f);
        LoadNextLevel();
    }
    
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            CompleteLevel();
    }
    
    void OnDestroy()
    {
        if (Larry == this)
            Larry = null;
    }
}

