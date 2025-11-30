using UnityEngine;
using UnityEngine.UIElements;

public class bpmCounter : MonoBehaviour
{

    [SerializeField] private int bpm;
    private int bpmCount = 0;
    private int currentBeat = 1;
    private float timeBetweenBeats;
    private float timeElapsed = 2f;
    private float timeAtLastBeat = 0;
    private float songPosition = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        songPosition = (float)AudioSettings.dspTime;
        timeBetweenBeats = 60f / (float)bpm;
        timeAtLastBeat = songPosition;
    }

    // Update is called once per frame
    void Update()
    {
        songPosition = (float)AudioSettings.dspTime;
        CountBPM();
    }

    private void CountBPM()
    {
        timeElapsed = songPosition - timeAtLastBeat;

        if (timeElapsed >= timeBetweenBeats)
        {
            timeElapsed = 0;
            timeAtLastBeat += timeBetweenBeats;
            bpmCount += 1;
            UpdateCurrentBeat();
        }
    }

    private void UpdateCurrentBeat()
    {
        currentBeat = bpmCount % 4;

        if (currentBeat == 0)
        {
            currentBeat = 4;
        }
    }
    public int GetBPM()
    {
        return bpm;
    }
    public int GetCurrentBeat()
    {
        return currentBeat;
    }

    public float GetTimeBetweenBeats()
    {
        return timeBetweenBeats;
    }

    public float GetSongPosition()
    {
        return songPosition;
    }

}
