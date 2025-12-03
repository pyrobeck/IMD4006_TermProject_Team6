using UnityEngine;

public class AutoLimitedZRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float minRotation = -230f;
    public float maxRotation = -130f;

    [Header("Speed Influence")]
    public float baseSpeed = 5f;
    public float speedMultiplier = 2f;
    public TapBPM tapBPM;

    [Header("Direction Control")]
    public bool reverseDirection = false; // ← Set in Inspector

    private float range;
    private float rotationProgress = 0f;

    void Start()
    {
        range = maxRotation - minRotation;
    }

    void Update()
    {
        float bpm = tapBPM != null ? tapBPM.GetBPM() : 132f;

        float dynamicSpeed = baseSpeed + ((bpm - 132f) * speedMultiplier);
        dynamicSpeed = Mathf.Max(0.1f, dynamicSpeed);

        rotationProgress += dynamicSpeed * Time.deltaTime * 2f;

        float t = Mathf.PingPong(rotationProgress, range);

        float angle = reverseDirection
            ? maxRotation - t    // reverse motion direction
            : minRotation + t;   // normal motion

        transform.localEulerAngles = new Vector3(0f, 0f, angle);
    }
}
