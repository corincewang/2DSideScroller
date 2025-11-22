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
        if (onlyRunInEditor && !Application.isEditor)
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

