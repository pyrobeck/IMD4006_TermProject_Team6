using UnityEngine;

public class BGColorChanger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color targetColor = Color.blue;

    [Header("Color Response Settings")]
    public float fadeSpeed = 5f;
    public float bpmColorFactor = 0.01f; // more → faster color shifting

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError(" BGColorChanger: No SpriteRenderer found on object!");
            enabled = false;
            return;
        }

        TapBPM.BPMUpdated += OnBPMChanged;
    }

    private void OnDestroy()
    {
        TapBPM.BPMUpdated -= OnBPMChanged;
    }

    private void Update()
    {
        // Smoothly fade into target color
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * fadeSpeed);
    }

    private void OnBPMChanged(int bpm)
    {
        // Convert BPM → HSV Hue value (loops smoothly)
        float hue = (bpm * bpmColorFactor) % 1f;
        targetColor = Color.HSVToRGB(hue, 1f, 1f);

        Debug.Log($" BG Color Updated — BPM {bpm} → Hue {hue:F2}");
    }
}
