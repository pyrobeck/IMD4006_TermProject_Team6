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

    private void Start()
    {
        StartTracks();
    }

    private void Update()
    {
        UpdateBassTrack();
        UpdateDrumTrack();
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
}
