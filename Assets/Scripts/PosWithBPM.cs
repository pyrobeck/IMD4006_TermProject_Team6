using UnityEngine;

public class PosWithBPM : MonoBehaviour
{
    public Transform targetObject; // The object to move
    public float minPosition = -20f;
    public float maxPosition = 20f;

    private float currentPosition;
    private float moveSpeed = 5f; // Smoothness factor
    private float targetPosX;

    void Start()
    {
        if (targetObject == null)
            targetObject = transform;

        TapBPM.BPMUpdated += OnBPMChanged;
    }

    void OnDestroy()
    {
        TapBPM.BPMUpdated -= OnBPMChanged;
    }

    void Update()
    {
        // Smoothly move toward the target position
        Vector3 newPos = targetObject.position;
        newPos.x = Mathf.Lerp(newPos.x, targetPosX, Time.deltaTime * moveSpeed);
        targetObject.position = newPos;
    }

    void OnBPMChanged(int bpm)
    {
        // Normalize BPM into [0, 800)
        bpm = Mathf.Clamp(bpm, 0, 800);

        // Map 0 → -20, 400 → +20, 800 → -20 again
        float t = (bpm % 800) / 400f; // 0–2 range
        if (t <= 1f)
            targetPosX = Mathf.Lerp(minPosition, maxPosition, t); // 0–400 bpm
        else
            targetPosX = Mathf.Lerp(maxPosition, minPosition, t - 1f); // 400–800 bpm
    }
}
