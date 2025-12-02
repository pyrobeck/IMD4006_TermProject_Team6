using UnityEngine;

public class setCameraBaselineTrigger : MonoBehaviour
{
    [SerializeField] cameraMovement camera;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Store the position of the checkpoint
        if (collision.CompareTag("Player"))
        {
            camera.SetNewCameraBaseline();


        }
    }
}
