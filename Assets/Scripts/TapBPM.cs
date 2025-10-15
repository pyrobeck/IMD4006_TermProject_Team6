using UnityEngine;

public class TapBPM : MonoBehaviour
{
    float currentTime;
    float previousTapTime = 0;
    float timeIntervalSincePreviousTap = 0;
    float averageTimeInterval = 0;

    int BPM = 0;
    int averageBPM = 0;

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            calculateAverageBPM();
        }
    }

    void calculateAverageBPM()
    {
        calculateAverageTimeInterval();

        BPM = (int)(60 / averageTimeInterval);

        averageBPM = (averageBPM + BPM) / 2;

        Debug.Log("BPM: " + averageBPM);
    }

    void calculateAverageTimeInterval()
    {
        currentTime = Time.time;
        timeIntervalSincePreviousTap = currentTime - previousTapTime;
        averageTimeInterval = (averageTimeInterval + timeIntervalSincePreviousTap) / 2;
        previousTapTime = currentTime;
    }

    int getBPM()  //for anything that needs to access the averageBPM in the future
    {
        return averageBPM;
    }
}
