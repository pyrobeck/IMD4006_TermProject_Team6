using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource; // for SFX
    public AudioSource audioSourceMusic;
    public AudioSource audioSourceBass;
    public AudioSource audioSourceDrums;

    public AudioClip music;
    public AudioClip bass;
    public AudioClip drums;

    public AudioClip jumpAudio;
    public AudioClip rollAudio;

    public float volume = 1.0f;
    private const float defaultBPM = 131f;

    private void OnEnable()
    {
        TapBPM.BPMUpdated += OnBPMChanged;
    }

    private void OnDisable()
    {
        TapBPM.BPMUpdated -= OnBPMChanged;
    }

    public void StartTracks()
    {
        SetupAndPlay(audioSourceMusic, music, 0.5f);
        SetupAndPlay(audioSourceBass, bass, 0f);
        SetupAndPlay(audioSourceDrums, drums, 0f);
    }

    private void SetupAndPlay(AudioSource source, AudioClip clip, float volume)
    {
        source.clip = clip;
        source.volume = volume;
        source.loop = true;
        source.pitch = 1f; // normal speed
        source.Play();
    }

    private void OnBPMChanged(int newBPM)
    {
        // Adjust playback speed relative to 131 BPM
        float pitchMultiplier = newBPM / defaultBPM;

        audioSourceMusic.pitch = pitchMultiplier;
        audioSourceBass.pitch = pitchMultiplier;
        audioSourceDrums.pitch = pitchMultiplier;

        Debug.Log($"[MusicController] Adjusted pitch to {pitchMultiplier:F2}x for BPM {newBPM}");
    }

    public void SetDrumVolume(float value)
    {
        audioSourceDrums.volume = Mathf.Clamp01(value);
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpAudio, volume);
    }

    public void PlayRollSound()
    {
        audioSource.PlayOneShot(rollAudio, volume);
    }
}
