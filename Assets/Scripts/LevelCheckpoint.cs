using UnityEngine;

public enum CheckpointType
{
    StartPoint,
    EndPoint,
    Both
}

[RequireComponent(typeof(Collider2D))]
public class LevelCheckpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public CheckpointType checkpointType = CheckpointType.StartPoint;
    
    [Header("Start Point Settings")]
    public Transform playerSpawnPoint;
    
    [Header("End Point Settings")]
    public bool isTrigger = true;
    
    [Header("Level Settings")]
    public int levelNumber = 1;
    
    private bool hasBeenReached = false;
    private Collider2D checkpointCollider;
    
    public static LevelCheckpoint currentStartPoint;
    
    void Start()
    {
        checkpointCollider = GetComponent<Collider2D>();
        
        if (checkpointType == CheckpointType.StartPoint || checkpointType == CheckpointType.Both)
        {
            currentStartPoint = this;
            SetupStartPoint();
        }
        
        if (checkpointType == CheckpointType.EndPoint || checkpointType == CheckpointType.Both)
        {
            SetupEndPoint();
        }
    }
    
    void OnDestroy()
    {
        if (currentStartPoint == this)
        {
            currentStartPoint = null;
        }
    }
    
    void SetupStartPoint()
    {
        if (playerSpawnPoint == null)
        {
            playerSpawnPoint = transform;
        }
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = playerSpawnPoint.position;
        }
    }
    
    public void RespawnPlayer(GameObject player)
    {
        if (playerSpawnPoint == null)
        {
            playerSpawnPoint = transform;
        }
        
        if (player != null)
        {
            player.transform.position = playerSpawnPoint.position;
            
            CharacterController2D controller = player.GetComponent<CharacterController2D>();
            if (controller != null)
            {
                controller.isAlive = true;
                controller.GetComponent<Collider2D>().enabled = true;
            }
            
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
    
    void SetupEndPoint()
    {
        if (checkpointCollider != null)
        {
            checkpointCollider.isTrigger = isTrigger;
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (checkpointType == CheckpointType.StartPoint) return;
        if (hasBeenReached) return;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterController2D playerController = collision.gameObject.GetComponent<CharacterController2D>();
            if (playerController != null && playerController.isAlive)
            {
                hasBeenReached = true;
                GameManager.Gary.CompleteLevel();
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        if (checkpointType == CheckpointType.StartPoint)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 1f);
        }
        else if (checkpointType == CheckpointType.EndPoint)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 1f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up * 1f, 
                          transform.position + Vector3.up * 1.5f + Vector3.right * 0.3f);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 1f);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.6f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up * 1f, 
                          transform.position + Vector3.up * 1.5f + Vector3.right * 0.3f);
        }
        
        #if UNITY_EDITOR
        if (UnityEditor.Selection.activeGameObject == gameObject || UnityEditor.Selection.activeGameObject == null)
        {
            UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, $"Level {levelNumber}");
        }
        #endif
    }
    
    public void ResetCheckpoint()
    {
        hasBeenReached = false;
    }
}

