using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public MovingPlatform platformScript; // Reference the platform's script

    [SerializeField] private bool isTargetTrigger = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTargetTrigger == true)
        {
            if (other.CompareTag("ThrownObject"))
            {
                platformScript.StartMovingDown();
            }
            return;
        }
        if (other.CompareTag("Player"))
        {
            platformScript.StartMovingDown();
            //Debug.Log("Player touched trigger: moving platform down.");
        }
    }
}
