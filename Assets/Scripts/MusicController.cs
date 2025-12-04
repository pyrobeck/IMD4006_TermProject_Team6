using UnityEngine;

public class MusicController : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player;

    [Header("Track Sources")]
    public AudioSource musicSource;
    public AudioSource bassSource;
    public AudioSource drumSource;

    [Header("Track Clips")]
    public AudioClip musicClip;
    public AudioClip bassClip;
    public AudioClip drumsClip;

    [Header("Bass Position Settings")]
    public float minX = -50f;
    public float maxX = 350f;
    public float minVolume = 0f;
    public float maxVolume = 0.8f;

    [Header("BPM Reference")]
    public TapBPM tapBPM;

    private const float defaultBPM = 132f;
    private float smoothPitchVelocity;

    private int lastBPM = 132; // store last BPM to detect changes

    private void Start()
    {
        StartTracks();
        UpdateMusicPitch(lastBPM); // initialize pitch
    }

    private void Update()
    {
        UpdateBassTrack();
        UpdateDrumTrack();

        if (tapBPM != null)
        {
            int currentBPM = tapBPM.GetBPM();

            // Only update if BPM changed
            if (currentBPM != lastBPM)
            {
                lastBPM = currentBPM;
                UpdateMusicPitch(currentBPM);
            }
        }
    }

    private void StartTracks()
    {
        musicSource.clip = musicClip;
        bassSource.clip = bassClip;
        drumSource.clip = drumsClip;

        musicSource.loop = bassSource.loop = drumSource.loop = true;

        musicSource.volume = 0.2f;
        bassSource.volume = 0f;
        drumSource.volume = 0f;

        musicSource.pitch = bassSource.pitch = drumSource.pitch = 1f;

        musicSource.Play();
        bassSource.Play();
        drumSource.Play();
    }

    private void UpdateBassTrack()
    {
        if (!player) return;

        float t = Mathf.InverseLerp(minX, maxX, player.position.x);
        bassSource.volume = Mathf.Lerp(minVolume, maxVolume, t);
    }

    private void UpdateDrumTrack()
    {
        if (!player) return;

        float speed = Mathf.Abs(player.GetComponent<Rigidbody2D>().linearVelocityX);
        float targetVolume = speed > 0.2f ? 0.8f : 0f;
        drumSource.volume = Mathf.Lerp(drumSource.volume, targetVolume, Time.deltaTime * 5f);
    }

    private void UpdateMusicPitch(int bpm)
    {
        // Clamp BPM to avoid extreme pitch values
        bpm = Mathf.Clamp(bpm, 20, 200);

        // 132 BPM = 1.0 pitch; adjust proportionally
        float targetPitch = bpm / defaultBPM;

        // Quick smoothing with Lerp for more responsiveness
        float smoothPitch = Mathf.Lerp(musicSource.pitch, targetPitch, 0.2f);

        // Apply pitch to all tracks
        musicSource.pitch = bassSource.pitch = drumSource.pitch = smoothPitch;

        // Log for debugging
        Debug.Log($"🎵 Pitch Updated → {smoothPitch:F2} | BPM → {bpm}");
    }

}
