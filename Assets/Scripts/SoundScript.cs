using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Steve;

    [Header("Background Music Per Level")]
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
            Destroy(this.gameObject);
        }
        else
        {
            Steve = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        thisAudio = GetComponent<AudioSource>();
        PlayBackgroundMusicForCurrentScene();
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBackgroundMusicForCurrentScene();
    }
    
    void PlayBackgroundMusicForCurrentScene()
    {
        if (thisAudio == null)
            return;
            
        string sceneName = SceneManager.GetActiveScene().name;
        AudioClip musicToPlay = null;
        
        if (sceneName == "level1" && level1BackgroundMusic != null)
        {
            musicToPlay = level1BackgroundMusic;
        }
        else if (sceneName == "level2" && level2BackgroundMusic != null)
        {
            musicToPlay = level2BackgroundMusic;
        }
        else if (sceneName == "level3" && level3BackgroundMusic != null)
        {
            musicToPlay = level3BackgroundMusic;
        }
        else if (sceneName == "BossScene" && bossBackgroundMusic != null)
        {
            musicToPlay = bossBackgroundMusic;
        }
        
        if (musicToPlay != null && thisAudio.clip != musicToPlay)
        {
            thisAudio.Stop();
            thisAudio.clip = musicToPlay;
            thisAudio.loop = true;
            thisAudio.Play();
        }
        else if (musicToPlay != null && !thisAudio.isPlaying)
        {
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
        if (levelCompleteSound != null)
        {
            thisAudio.PlayOneShot(levelCompleteSound);
        }
    }
}
