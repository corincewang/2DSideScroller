using UnityEngine;

public class FerryPlatform : MonoBehaviour
{
    private Animator animator;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        
        animator = GetComponent<Animator>();
        
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }
        
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("FerryStart");
        }
    }
    
    public void ResetPlatform()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        
        animator.Rebind();
        animator.Update(0f);
    }
}
