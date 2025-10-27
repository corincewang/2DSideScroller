using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 1f;
    public float currentVelocity = 0f;
    public float currentVelocityY = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
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
        
        cameraPosition.y = Mathf.SmoothDamp(cameraPosition.y, player.position.y, ref currentVelocityY, smoothTime);
        
        transform.position = cameraPosition;
    }
}
