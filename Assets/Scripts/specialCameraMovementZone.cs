using Unity.VisualScripting;
using UnityEngine;

public class specialCameraMovementZone : MonoBehaviour
{
    [SerializeField] private Vector2 additionalCameraMovement;
    [SerializeField] private cameraMovement camera;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Store the position of the checkpoint
        if (collision.CompareTag("Player"))
        {
            camera.SpecialCameraZone(additionalCameraMovement);
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            camera.SpecialCameraZoneExit();
        }
    }

}
