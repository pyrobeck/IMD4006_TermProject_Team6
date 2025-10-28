using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speedBackground = 0.05f;

    private void Update()
    {
        if (cameraTransform == null)
            return;

        // Calculate the new background position
        Vector3 newBackgroundPosition = new Vector3(
            cameraTransform.position.x * speedBackground,
            cameraTransform.position.y * speedBackground,
            transform.position.z // keep original depth
        );

        // Apply the new position
        transform.position = newBackgroundPosition;
    }
}
