using UnityEngine;

public enum CheckpointType
{
    StartPoint,
    EndPoint,
    Both,
    RespawnCheckpoint
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
    
    [Header("Respawn Checkpoint Settings")]
    public float checkpointX;
    public float checkpointY;
    public bool useTriggerForCheckpoint = false;
    
    [Header("Level Settings")]
    public int levelNumber = 1;
    
    private bool hasBeenReached = false;
    private bool hasBeenActivated = false;
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
        
        if (checkpointType == CheckpointType.RespawnCheckpoint)
        {
            SetupRespawnCheckpoint();
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
            playerSpawnPoint = transform;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = playerSpawnPoint.position;
        }
    }
    
    public void RespawnPlayer(GameObject player)
    {
        if (playerSpawnPoint == null)
            playerSpawnPoint = transform;
        
        player.transform.position = playerSpawnPoint.position;
        
        CharacterController2D controller = player.GetComponent<CharacterController2D>();
        controller.isAlive = true;
        controller.GetComponent<Collider2D>().enabled = true;
        
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
    }
    
    void SetupEndPoint()
    {
        checkpointCollider.isTrigger = isTrigger;
    }
    
    void SetupRespawnCheckpoint()
    {
        if (useTriggerForCheckpoint)
            checkpointCollider.isTrigger = true;
        else
            checkpointCollider.enabled = false;
    }
    
    void Update()
    {
        if (checkpointType == CheckpointType.RespawnCheckpoint && !useTriggerForCheckpoint && !hasBeenActivated)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                CharacterController2D playerController = player.GetComponent<CharacterController2D>();
                if (playerController != null && playerController.isAlive)
                {
                    if (player.transform.position.x >= checkpointX)
                    {
                        ActivateRespawnCheckpoint();
                    }
                }
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (checkpointType == CheckpointType.StartPoint) return;
        
        if (checkpointType == CheckpointType.RespawnCheckpoint)
        {
            if (!useTriggerForCheckpoint || hasBeenActivated) return;
            
            if (collision.gameObject.CompareTag("Player"))
            {
                CharacterController2D playerController = collision.gameObject.GetComponent<CharacterController2D>();
                if (playerController != null && playerController.isAlive)
                {
                    ActivateRespawnCheckpoint();
                }
            }
            return;
        }
        
        if (hasBeenReached) return;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterController2D playerController = collision.gameObject.GetComponent<CharacterController2D>();
            if (playerController != null && playerController.isAlive)
            {
                hasBeenReached = true;
                if (LevelScript.Larry != null)
                    LevelScript.Larry.CompleteLevel();
            }
        }
    }
    
    void ActivateRespawnCheckpoint()
    {
        if (hasBeenActivated) return;
        
        hasBeenActivated = true;
        
        if (GameManager.Gary != null)
        {
            Vector3 checkpointPos = new Vector3(checkpointX, checkpointY, transform.position.z);
            GameManager.Gary.SetCheckpointPosition(checkpointPos);
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
        else if (checkpointType == CheckpointType.RespawnCheckpoint)
        {
            Gizmos.color = hasBeenActivated ? Color.green : Color.cyan;
            
            if (useTriggerForCheckpoint)
            {
                Gizmos.DrawWireSphere(transform.position, 0.5f);
            }
            else
            {
                Vector3 checkpointPos = new Vector3(checkpointX, checkpointY, transform.position.z);
                Gizmos.DrawWireSphere(checkpointPos, 0.5f);
                Gizmos.DrawLine(transform.position, checkpointPos);
            }
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
            if (checkpointType == CheckpointType.RespawnCheckpoint)
            {
                Vector3 labelPos = useTriggerForCheckpoint ? transform.position : new Vector3(checkpointX, checkpointY, transform.position.z);
                UnityEditor.Handles.Label(labelPos + Vector3.up * 1f, $"Respawn Checkpoint\nX: {checkpointX}, Y: {checkpointY}");
            }
            else
            {
                UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, $"Level {levelNumber}");
            }
        }
        #endif
    }
    
    public void ResetCheckpoint()
    {
        hasBeenReached = false;
        hasBeenActivated = false;
    }
}

