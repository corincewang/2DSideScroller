using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Steve;

    public AudioClip backgroundMusic;
    public AudioClip playerDeathSound;
    public AudioClip playerJumpSound;
    public AudioClip enemyHitSound;
    
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
        }
    }

    void Start()
    {
        thisAudio = GetComponent<AudioSource>();
        
        if (backgroundMusic != null)
        {
            thisAudio.clip = backgroundMusic;
            thisAudio.loop = true;
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
}
