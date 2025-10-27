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

    public void StartTracks()
    {
        audioSourceMusic.clip = music;
        audioSourceMusic.volume = 0.5f;
        audioSourceMusic.loop = true;
        audioSourceMusic.Play();

        audioSourceBass.clip = bass;
        audioSourceBass.volume = 0f;
        audioSourceBass.loop = true;
        audioSourceBass.Play();

        audioSourceDrums.clip = drums;
        audioSourceDrums.volume = 0f;
        audioSourceDrums.loop = true;
        audioSourceDrums.Play();
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
