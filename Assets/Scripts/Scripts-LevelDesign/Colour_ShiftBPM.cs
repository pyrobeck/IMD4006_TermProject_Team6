using UnityEngine;

public class Colour_ShiftWindow : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("BPM Linking")]
    public TapBPM tapBPM;
    private float currentBPM = 132f;

    [Header("Speed Mapping")]
    public float minBPM = 60f;
    public float maxBPM = 400;
    public float minShiftSpeed = 0.05f;
    public float maxShiftSpeed = 0.4f;

    private float hueValue = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!spriteRenderer)
        {
            Debug.LogError("❌ Colour_ShiftWindow: No SpriteRenderer found!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (tapBPM != null)
            currentBPM = tapBPM.GetBPM();

        // Normalize BPM to 0–1 based on min/max range
        float t = Mathf.InverseLerp(minBPM, maxBPM, currentBPM);

        // Use smooth interpolation for speed
        float shiftSpeed = Mathf.Lerp(minShiftSpeed, maxShiftSpeed, t);

        // Update hue
        hueValue += shiftSpeed * Time.deltaTime;
        if (hueValue >= 1f) hueValue -= 1f;

        spriteRenderer.color = Color.HSVToRGB(hueValue, 1f, 1f);
    }
}
