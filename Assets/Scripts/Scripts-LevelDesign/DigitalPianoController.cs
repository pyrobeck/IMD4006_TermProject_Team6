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
    { KeyCode.A, 0 },
    { KeyCode.Z, 0 },

    { KeyCode.S, 1 },
    { KeyCode.X, 1 },
    { KeyCode.LeftAlt, 1 },
    { KeyCode.RightAlt, 1 },

    { KeyCode.C, 2 },
    { KeyCode.R, 2 },
    { KeyCode.F, 2 },

    { KeyCode.V, 3 },

    { KeyCode.B, 4 },      // was R → now B N Space
    { KeyCode.N, 4 },
    { KeyCode.Space, 4 },

    { KeyCode.M, 5 },      // was T → now M

    { KeyCode.Comma, 6 },  // was Y → now , . L O
    { KeyCode.Period, 6 },
    { KeyCode.L, 6 },
    { KeyCode.O, 6 },

    { KeyCode.P, 7 },      // was U → now P ; /
    { KeyCode.Semicolon, 7 },
    { KeyCode.Slash, 7 },

    { KeyCode.RightBracket, 8 }, // was I → now ] Shift
    { KeyCode.LeftShift, 8 },
    { KeyCode.RightShift, 8 },

    { KeyCode.Return, 9 }, // was O → now Enter

    { KeyCode.LeftControl, 10 }, // was P → now Ctrl
    { KeyCode.RightControl, 10 },

    { KeyCode.UpArrow, 11 },     // was LeftBracket → now Up, Left, Down
    { KeyCode.LeftArrow, 11 },
    { KeyCode.DownArrow, 11 },

    { KeyCode.RightArrow, 12 },  // was RightBracket → now Right arrow

    { KeyCode.Alpha0, 13 },      // was Backslash → now "10"? You wrote "10", mapping to key "0"
    
    // -------------------------
    // Black Keys (10)
    // -------------------------
    
    // was 1 → now Q W
    { KeyCode.Q, 14 },
    { KeyCode.W, 14 },

    // was 2 → now E
    { KeyCode.E, 15 },

    // was 3 → now Y
    { KeyCode.Y, 16 },

    // was 4 → now U J
    { KeyCode.U, 17 },
    { KeyCode.J, 17 },

    // was 5 → now K L
    { KeyCode.K, 18 },

    // was 6 → now [
    { KeyCode.LeftBracket, 19 },

    // was 7 → now Enter + Backslash
    { KeyCode.Backslash, 20 },

    // was 8 → now Delete
    { KeyCode.Delete, 21 },

    // was 9 → now Page Up + Page Down
    { KeyCode.PageUp, 22 },
    { KeyCode.PageDown, 22 },

    // was 0 → now Key 7
    { KeyCode.Alpha7, 23 },
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
