using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speedBackground = 0.05f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (cameraTransform == null)
            return;

        Vector3 cameraOffset = cameraTransform.position * speedBackground;
        transform.position = startPosition + new Vector3(cameraOffset.x, cameraOffset.y, 0);
    }
}
