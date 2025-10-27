using UnityEngine;

public class PosWithBPM : MonoBehaviour
{
    public Transform targetObject; // The object to move
    public float minPosition = -20f;
    public float maxPosition = 20f;

    private float moveSpeed = 5f; // Smoothness factor
    private float targetPosX;

    void Start()
    {
        if (targetObject == null)
            targetObject = transform;

        TapBPM.BPMUpdated += OnBPMChanged;

        // Apply default BPM immediately
        OnBPMChanged(131);
    }


    void OnDestroy() => TapBPM.BPMUpdated -= OnBPMChanged;

    void Update()
    {
        Vector3 newPos = targetObject.position;
        newPos.x = Mathf.Lerp(newPos.x, targetPosX, Time.deltaTime * moveSpeed);
        targetObject.position = newPos;
    }

    void OnBPMChanged(int bpm)
    {
        bpm = Mathf.Clamp(bpm, 0, 800);
        float t = (bpm % 800) / 400f;
        targetPosX = (t <= 1f) ? Mathf.Lerp(minPosition, maxPosition, t)
                               : Mathf.Lerp(maxPosition, minPosition, t - 1f);
    }
}
