using UnityEngine;
using System.Collections.Generic;

public class NoteVisualSpawner : MonoBehaviour
{
    [Header("Visual Settings")]
    public GameObject noteSpritePrefab;
    public Transform spawnArea;
    public float riseSpeed = 1.5f;
    public float fadeSpeed = 1.5f;

    [Header("Random Spawn Settings")]
    public float spawnRadius = 3.0f;     // how far from center notes can appear

    [Header("Note Data Link")]
    public DigitalPianoController piano;

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

        // 🔥 Random XY offset inside a circle
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;

        Vector3 spawnPos = new Vector3(
            spawnArea.position.x + randomOffset.x,
            spawnArea.position.y + randomOffset.y,
            spawnArea.position.z
        );

        GameObject note = Instantiate(
            noteSpritePrefab,
            spawnPos,
            Quaternion.identity,
            spawnArea
        );

        SpriteRenderer sr = note.GetComponent<SpriteRenderer>();

        // Color mapping based on note index
        float hue = (noteIndex / 24f) % 1f;
        sr.color = Color.HSVToRGB(hue, 1f, 1f);

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
