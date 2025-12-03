using UnityEngine;
using System.Collections.Generic;

public class DigitalPianoController : MonoBehaviour
{
    // 🔔 Event for visualizers (like NoteVisualSpawner)
    public static event System.Action<int> NotePlayed;

    [Header("Audio Clips (24 Notes)")]
    public AudioClip[] pianoNotes; // Size 24: 14 white + 10 black

    private AudioSource audioSource;
    private Dictionary<KeyCode, int> noteMap;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Assign keys to note index in your AudioClip array
        noteMap = new Dictionary<KeyCode, int>()
        {
            // White Keys (14)
            { KeyCode.Tab, 0 },
            { KeyCode.Q, 1 },
            { KeyCode.W, 2 },
            { KeyCode.E, 3 },
            { KeyCode.R, 4 },
            { KeyCode.T, 5 },
            { KeyCode.Y, 6 },
            { KeyCode.U, 7 },
            { KeyCode.I, 8 },
            { KeyCode.O, 9 },
            { KeyCode.P, 10 },
            { KeyCode.LeftBracket, 11 },
            { KeyCode.RightBracket, 12 },
            { KeyCode.Backslash, 13 },

            // Black Keys (10)
            { KeyCode.Alpha1, 14 },
            { KeyCode.Alpha2, 15 },
            { KeyCode.Alpha3, 16 },
            { KeyCode.Alpha4, 17 },
            { KeyCode.Alpha5, 18 },
            { KeyCode.Alpha6, 19 },
            { KeyCode.Alpha7, 20 },
            { KeyCode.Alpha8, 21 },
            { KeyCode.Alpha9, 22 },
            { KeyCode.Alpha0, 23 }
        };
    }

    void Update()
    {
        foreach (var note in noteMap)
        {
            if (Input.GetKeyDown(note.Key))
                PlayNote(note.Value);
        }
    }

    void PlayNote(int index)
    {
        if (index < 0 || index >= pianoNotes.Length) return;

        AudioClip clip = pianoNotes[index];
        if (clip)
            audioSource.PlayOneShot(clip);

        // 📣 Tell listeners a note was played!
        NotePlayed?.Invoke(index);
    }
}
