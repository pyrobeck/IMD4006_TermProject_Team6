using UnityEngine;

public class PlatformMoveWithBPMY : MonoBehaviour
{
    public Transform platform;       // Object to move
    public float minSpeed = 1f;    // speed at BPM 1
    public float maxSpeed = 4f;      // speed at BPM 800
    public float minY = -20f;        // left boundary
    public float maxY = 20f;         // right boundary

    private float currentSpeed = 1.0f;
    private int direction = 1;       // 1 = right, -1 = left

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
        // Move platform along y
        Vector3 pos = platform.position;
        pos.y += currentSpeed * direction * Time.deltaTime;

        // Check boundaries and flip direction
        if (pos.y >= maxY)
        {
            pos.y = maxY;
            direction = -1;
        }
        else if (pos.y <= minY)
        {
            pos.y = minY;
            direction = 1;
        }

        platform.position = pos;
    }

    void OnBPMChanged(int bpm)
    {
        // Map BPM 1-800 → speed 0.1-2, clamp max speed
        bpm = Mathf.Max(bpm, 1); // prevent 0
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, bpm / 800f);
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
    }
}
