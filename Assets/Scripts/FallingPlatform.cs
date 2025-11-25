using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public MovingPlatform platformScript; // Reference the platform's script

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            platformScript.StartMovingDown();
            Debug.Log("Player touched trigger: moving platform down.");
        }
    }
}
