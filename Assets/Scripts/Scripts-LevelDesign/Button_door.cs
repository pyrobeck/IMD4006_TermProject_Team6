using UnityEngine;

public class Button_door : MonoBehaviour
{
    [SerializeField] private GameObject targetToDelete;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ThrownObject"))
        {
            if (targetToDelete != null)
            {
                Destroy(targetToDelete);
            }
        }
    }
}
