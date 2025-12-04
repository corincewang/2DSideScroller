using UnityEngine;

public class SceneBootstrap : MonoBehaviour
{
    [Header("Required Prefabs")]
    public GameObject gameManagerPrefab;
    public GameObject soundManagerPrefab;
    
    [Header("Settings")]
    public bool onlyRunInEditor = true;
    
    void Awake()
    {
        bool isLevel1 = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0;
        
        if (onlyRunInEditor && !Application.isEditor && !isLevel1)
        {
            return;
        }
        
        if (GameManager.Gary == null)
        {
            if (gameManagerPrefab != null)
            {
                Instantiate(gameManagerPrefab);
            }
            else
            {
                CreateEmptyGameManager();
            }
        }
        
        if (LevelScript.Larry == null)
        {
            CreateLevelScript();
        }
        
        if (SoundManager.Steve == null)
        {
            if (soundManagerPrefab != null)
            {
                Instantiate(soundManagerPrefab);
            }
        }
    }
    
    void CreateEmptyGameManager()
    {
        GameObject gmObj = new GameObject("GameManager");
        gmObj.AddComponent<GameManager>();
        DontDestroyOnLoad(gmObj);
    }
    
    void CreateLevelScript()
    {
        GameObject lsObj = new GameObject("LevelScript");
        lsObj.AddComponent<LevelScript>();
        DontDestroyOnLoad(lsObj);
    }
}

