using UnityEngine;

public class NewCheckpoint : MonoBehaviour
{
// [Header("Object to show when checkpoint activates")]
    public GameObject targetObject;

    private bool isActive = false;

    private void Start()
    {
        // Make sure the target is hidden at the start
        if (targetObject != null)
            targetObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive && collision.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }
    public void SetActive() { 
        isActive = true; 
        } 
    
    public void SetInactive() { 
        isActive = false; 
    } 

    private void ActivateCheckpoint()
    {
        isActive = true;

        // Make the target object appear
        if (targetObject != null)
            targetObject.SetActive(true);

        Debug.Log("Checkpoint Activated!");
    }
}
