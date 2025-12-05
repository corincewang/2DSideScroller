using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Steve;

    [Header("Background Music Per Level")]
    public AudioClip menuBackgroundMusic;
    public AudioClip level1BackgroundMusic;
    public AudioClip level2BackgroundMusic;
    public AudioClip level3BackgroundMusic;
    public AudioClip bossBackgroundMusic;
    
    [Header("Sound Effects")]
    public AudioClip playerDeathSound;
    public AudioClip playerJumpSound;
    public AudioClip enemyHitSound;
    public AudioClip starSound;
    public AudioClip levelCompleteSound;
    
    private AudioSource thisAudio;

    private void Awake()
    {
        if (Steve && Steve != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Steve = this;
            DontDestroyOnLoad(gameObject);
            thisAudio = GetComponent<AudioSource>();
        }
    }

    void Start()
    {
    }
    
    public void PlayBackgroundMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        AudioClip musicToPlay = null;
        
        if (sceneName == "MenuScene") musicToPlay = menuBackgroundMusic;
        else if (sceneName == "level1") musicToPlay = level1BackgroundMusic;
        else if (sceneName == "level2") musicToPlay = level2BackgroundMusic;
        else if (sceneName == "level3") musicToPlay = level3BackgroundMusic;
        else if (sceneName == "level4") musicToPlay = bossBackgroundMusic;
        
        if (musicToPlay != null && (thisAudio.clip != musicToPlay || !thisAudio.isPlaying))
        {
            thisAudio.Stop();
            thisAudio.clip = musicToPlay;
            thisAudio.Play();
        }
    }

    public void PlayDeathSound()
    {
        thisAudio.PlayOneShot(playerDeathSound);
    }

    public void PlayJumpSound()
    {
        thisAudio.PlayOneShot(playerJumpSound);
    }

    public void PlayEnemyHitSound()
    {
        thisAudio.PlayOneShot(enemyHitSound);
    }
    
    public void PlayStarSound()
    {
        thisAudio.PlayOneShot(starSound);
    }
    
    public void PlayLevelCompleteSound()
    {
        thisAudio.PlayOneShot(levelCompleteSound);
    }
}
