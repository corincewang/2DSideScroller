using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Gary != null)
        {
            if (GameManager.Gary.scoreDisplay) GameManager.Gary.scoreDisplay.enabled = false;
            if (GameManager.Gary.livesDisplay) GameManager.Gary.livesDisplay.enabled = false;
            if (GameManager.Gary.timerDisplay) GameManager.Gary.timerDisplay.enabled = false;
            if (GameManager.Gary.messageOverlay) GameManager.Gary.messageOverlay.enabled = false;
        }
        
        if (SoundManager.Steve)
        {
            SoundManager.Steve.PlayBackgroundMusicForCurrentScene();
        }
            
    }
    
    // most menuscript referred to AstralAttacker's menuScipt
    public void btn_StartTheGame()
    {
        if (GameManager.Gary != null)
        {
            GameManager.Gary.ResetGameState();
        }
        SceneManager.LoadScene("level1");
    }
    
    public void btn_GoToTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }
    
    public void btn_GoToCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }
    
    public void btn_GoBackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    
    public void btn_QuitGame()
    {
        Application.Quit();
    }
}

