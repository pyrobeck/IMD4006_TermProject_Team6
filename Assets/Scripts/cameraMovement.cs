using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float zoom = 7f;
    [SerializeField] float verticalFollowSmoothness = 3f; // Smooth damp for Y tracking

    Vector3 zOffset = new Vector3(0, 0, -10);
    Vector3 lookAheadOffset;
    float lookAheadOffsetX;
    int direction = 1;

    float currentYVelocity; // Used for smooth damping

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.orthographicSize = zoom;

        lookAheadOffsetX = (zoom / 3f) + zoom; // forward view distance
    }

    void Update()
    {
        UpdateDirection();
        UpdateLookAheadOffset();

        // Smoothly follow player Y position
        float targetY = Mathf.SmoothDamp(transform.position.y, target.position.y + zoom / 3f, ref currentYVelocity, 1f / verticalFollowSmoothness);

        // Target X position follows player, Y follows smoothly, Z stays fixed
        Vector3 targetPos = new Vector3(target.position.x, targetY, 0) + lookAheadOffset + zOffset;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void UpdateDirection()
    {
        // Flip lookahead depending on player facing direction
        direction = target.localScale.x < 0 ? -1 : 1;
    }

    private void UpdateLookAheadOffset()
    {
        lookAheadOffset = new Vector3(lookAheadOffsetX * direction, 0, 0);
    }

       public void SnapToTarget()
    {
        UpdateDirection();
        UpdateLookAheadOffset();

        // Instantly set camera position to target’s location
        Vector3 snapPos = new Vector3(target.position.x, target.position.y + zoom / 3f, 0) + lookAheadOffset + zOffset;
        transform.position = snapPos;
    }
}
