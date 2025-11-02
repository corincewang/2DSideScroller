using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 1f;
    public float currentVelocity = 0f;
    public float currentVelocityY = 0f;
    public float yOffset = -1f;
    public float maxY = 3.5f;
    public float minY = 2f;
    public float minX = 0.68f;
    public float maxX = 133f;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 cameraPosition = transform.position;

        float targetX = Mathf.Clamp(player.position.x, minX, maxX);
        cameraPosition.x = Mathf.SmoothDamp(cameraPosition.x, targetX, ref currentVelocity, smoothTime);
        
        float targetY = player.position.y + yOffset;
        targetY = Mathf.Clamp(targetY, minY, maxY);
        cameraPosition.y = Mathf.SmoothDamp(cameraPosition.y, targetY, ref currentVelocityY, smoothTime);
        
        transform.position = cameraPosition;
    }
}
