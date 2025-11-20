using UnityEngine;

public class PlatformMoveWithBPMBounce : MonoBehaviour
{
    public Transform platform;       // Object to move
    public float minSpeed;    // speed at BPM 1
    public float maxSpeed;      // speed at BPM 800
    public float minX = -20f;        // left boundary
    public float maxX = 20f;         // right boundary

    private float currentSpeed = 3f;
    private int direction = 1;       // 1 = right, -1 = left

    public Vector3 DeltaPosition { get; private set; }

    void Start()
    {
        if (platform == null)
            platform = transform;

        TapBPM.BPMUpdated += OnBPMChanged;
    }

    void OnDestroy()
    {
        TapBPM.BPMUpdated -= OnBPMChanged;
    }

    void Update()
    {
        Vector3 oldPos = platform.position;
        // Move platform along X
        Vector3 pos = platform.position;
        pos.x += currentSpeed * direction * Time.deltaTime;

        // Check boundaries and flip direction
        if (pos.x >= maxX)
        {
            pos.x = maxX;
            direction = -1;
        }
        else if (pos.x <= minX)
        {
            pos.x = minX;
            direction = 1;
        }

        platform.position = pos;
        DeltaPosition = platform.position - oldPos;
    }

    void OnBPMChanged(int bpm)
    {
        // Map BPM 1-800 → speed 0.1-2, clamp max speed
        bpm = Mathf.Max(bpm, 1); // prevent 0
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, bpm / 800f);
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
    }
}
