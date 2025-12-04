using Unity.VisualScripting;
using UnityEngine;

public class specialCameraMovementZone : MonoBehaviour
{
    [SerializeField] private Vector2 additionalCameraMovement;
    [SerializeField] private bool lockCamera;
    [SerializeField] private cameraMovement camera;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (lockCamera == true)
            {
                camera.LockCamera(additionalCameraMovement);
            }
            else
            {
                camera.SpecialCameraZone(additionalCameraMovement);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (lockCamera == true)
            {
                camera.UnlockCamera();
            }
            else
            {
                camera.SpecialCameraZoneExit();
            }
        }
    }
}
