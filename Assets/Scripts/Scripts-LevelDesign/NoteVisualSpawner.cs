using UnityEngine;
using System.Collections.Generic;



public class NoteVisualSpawner : MonoBehaviour
{
    [Header("Visual Settings")]
    public GameObject noteSpritePrefab;
    public Transform spawnArea; // place above keyboard visually
    public float riseSpeed = 1.5f;
    public float fadeSpeed = 1.5f;
    public static event System.Action<int> NotePlayed;


    [Header("Note Data Link")]
    public DigitalPianoController piano; // drag your piano script object

    private void OnEnable()
    {
        DigitalPianoController.NotePlayed += SpawnNoteVisual;
    }

    private void OnDisable()
    {
        DigitalPianoController.NotePlayed -= SpawnNoteVisual;
    }

    private void SpawnNoteVisual(int noteIndex)
    {
        if (!noteSpritePrefab || !spawnArea) return;

        GameObject note = Instantiate(
            noteSpritePrefab,
            spawnArea.position,
            Quaternion.identity,
            spawnArea
        );

        SpriteRenderer sr = note.GetComponent<SpriteRenderer>();

        // Assign color based on note index (rainbow mapping)
        float hue = (noteIndex / 24f) % 1f;
        Color noteColor = Color.HSVToRGB(hue, 1f, 1f);
        sr.color = noteColor;

        // Animate + Self-Destruct
        StartCoroutine(AnimateNote(note, sr));
    }

    private System.Collections.IEnumerator AnimateNote(GameObject note, SpriteRenderer sr)
    {
        float alpha = 1f;

        while (alpha > 0f)
        {
            note.transform.position += Vector3.up * riseSpeed * Time.deltaTime;

            alpha -= fadeSpeed * Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

            yield return null;
        }

        Destroy(note);
    }
}
