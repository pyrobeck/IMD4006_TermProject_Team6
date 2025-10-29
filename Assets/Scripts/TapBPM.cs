using UnityEngine;

public class TapBPM : MonoBehaviour
{
    float currentTime;
    float previousTapTime = 0;
    float timeIntervalSincePreviousTap = 0;
    float averageTimeInterval = 0;

    int BPM = 0;
    public int averageBPM = 0;

    public delegate void OnBPMUpdated(int bpm);
    public static event OnBPMUpdated BPMUpdated;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    if (!key.ToString().StartsWith("Joystick") &&
                        !key.ToString().StartsWith("Mouse"))
                    {
                        CalculateAverageBPM();
                        BPMUpdated?.Invoke(averageBPM); // Notify listeners
                        break;
                    }
                }
            }
        }
    }

    void CalculateAverageBPM()
    {
        CalculateAverageTimeInterval();

        BPM = (int)(60 / averageTimeInterval);
        averageBPM = (averageBPM + BPM) / 2;

        Debug.Log("BPM: " + averageBPM);
    }

    void CalculateAverageTimeInterval()
    {
        currentTime = Time.time;
        timeIntervalSincePreviousTap = currentTime - previousTapTime;

        if (previousTapTime == 0)
            averageTimeInterval = timeIntervalSincePreviousTap;
        else
            averageTimeInterval = (averageTimeInterval + timeIntervalSincePreviousTap) / 2;

        previousTapTime = currentTime;
    }

    public int GetBPM()
    {
        return averageBPM;
    }
}
