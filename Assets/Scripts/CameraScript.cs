using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 1f;
    public float currentVelocity = 0f;
    public float currentVelocityY = 0f;
    public float yOffset = -1f;
    public float maxY = 5f;
    public float minY = 0f;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 cameraPosition = transform.position;

        if (player.position.x > cameraPosition.x)
        {
            cameraPosition.x = Mathf.SmoothDamp(cameraPosition.x, player.position.x, ref currentVelocity, smoothTime);
        }
        else
        {
            currentVelocity = 0;
        }
        
        float targetY = player.position.y + yOffset;
        targetY = Mathf.Clamp(targetY, minY, maxY);
        cameraPosition.y = Mathf.SmoothDamp(cameraPosition.y, targetY, ref currentVelocityY, smoothTime);
        
        transform.position = cameraPosition;
    }
}
