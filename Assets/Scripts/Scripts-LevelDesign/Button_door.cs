using UnityEngine;

public class Button_door : MonoBehaviour
{
    [SerializeField] private GameObject targetToDelete;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ThrownObject"))
        {
            if (targetToDelete != null)
            {
                Destroy(targetToDelete);
            }
        }
    }
}
