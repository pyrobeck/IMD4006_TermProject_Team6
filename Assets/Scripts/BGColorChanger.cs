using UnityEngine;

public class BGColorChanger : MonoBehaviour
{
    private GameObject[] bgElements;
    private Color targetColor = Color.blue;
    private float fadeSpeed = 3f;

    void Start()
    {
        bgElements = GameObject.FindGameObjectsWithTag("BGElements");
        TapBPM.BPMUpdated += OnBPMChanged; // Subscribe to BPM updates
    }

    void OnDestroy()
    {
        TapBPM.BPMUpdated -= OnBPMChanged; // Unsubscribe (good practice)
    }

    void Update()
    {
        FadeBGColors();
    }

    void OnBPMChanged(int bpm)
    {
        // Map BPM modulo 300 to a color range
        float bpmLoop = bpm % 300f;
        float t = bpmLoop / 300f;

        // Create a gradient-like color mapping
        if (t < 0.25f)
            targetColor = Color.Lerp(Color.blue, Color.cyan, t / 0.25f);
        else if (t < 0.5f)
            targetColor = Color.Lerp(Color.cyan, Color.green, (t - 0.25f) / 0.25f);
        else if (t < 0.75f)
            targetColor = Color.Lerp(Color.green, Color.yellow, (t - 0.5f) / 0.25f);
        else
            targetColor = Color.Lerp(Color.yellow, Color.red, (t - 0.75f) / 0.25f);
    }

    void FadeBGColors()
    {
        foreach (GameObject obj in bgElements)
        {
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color.Lerp(sr.color, targetColor, Time.deltaTime * fadeSpeed);
            }
        }
    }
}
