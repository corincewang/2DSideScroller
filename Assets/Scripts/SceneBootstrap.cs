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
        // 如果是level1 (buildIndex 0)，总是运行bootstrap
        // 其他关卡只在编辑器中运行bootstrap（用于测试单个场景）
        bool isLevel1 = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0;
        
        if (onlyRunInEditor && !Application.isEditor && !isLevel1)
        {
            return;
        }
        
        if (GameManager.Gary == null)
        {
            if (gameManagerPrefab != null)
            {
                Debug.Log("[Bootstrap] Creating GameManager");
                Instantiate(gameManagerPrefab);
            }
            else
            {
                Debug.LogWarning("[Bootstrap] GameManager Prefab not set!");
                CreateEmptyGameManager();
            }
        }
        
        if (LevelScript.Larry == null)
        {
            Debug.Log("[Bootstrap] Creating LevelScript");
            CreateLevelScript();
        }
        
        if (SoundManager.Steve == null)
        {
            if (soundManagerPrefab != null)
            {
                Debug.Log("[Bootstrap] Creating SoundManager");
                Instantiate(soundManagerPrefab);
            }
            else
            {
                Debug.LogWarning("[Bootstrap] SoundManager Prefab not set! Audio will be unavailable.");
            }
        }
        
        Debug.Log("[Bootstrap] Scene initialization complete");
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

