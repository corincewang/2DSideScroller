using UnityEngine;
using UnityEngine.SceneManagement;
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
        GameManager.Gary.LevelStarted();
    }
    
    public void CompleteLevel()
    {
        levelCompleted = true;
        StartCoroutine(LevelWin());
    }
    
    IEnumerator LevelWin()
    {
        GameManager.Gary.messageOverlay.enabled = true;
        GameManager.Gary.messageOverlay.text = "Level Cleared!!";
        
        if (SoundManager.Steve)
        {
            SoundManager.Steve.GetComponent<AudioSource>().Stop();
            SoundManager.Steve.PlayLevelCompleteSound();
        }
        
        yield return new WaitForSeconds(5f);
        LoadNextLevel();
    }
    
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player"){
            CompleteLevel();
        }
            
    }
    
    void OnDestroy()
    {
        if (Larry == this){
            Larry = null;
        }
    }
}

