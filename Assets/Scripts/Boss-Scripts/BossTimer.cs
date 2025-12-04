using UnityEngine;
using UnityEngine.Events;

public class BossTimer : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private Transform timerHands;
    public UnityEvent Failure;
    private float timeRemaining;
    private Transform timerBar;
    bool isTimerRunning = false;
    private float timerBarLength;
    private float timerBarShrinkAmount;
    private Vector3 timerBarTargetScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerBar = transform.GetChild(0);
        timerBarLength = timerBar.localScale.x;
        timerBarShrinkAmount = timerBarLength / time;
        timerBarTargetScale = timerBar.localScale;
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        DecrementTimer();
        ShrinkTimerBar();
        SpinTimerHands();
    }

    private void DecrementTimer()
    {
        if (isTimerRunning)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                isTimerRunning = false;
                Failure.Invoke();
            }
        }
    }

    private void ShrinkTimerBar()
    {
        if (isTimerRunning)
        {

            if (timerBar.localScale.x <= 0)
            {
                timerBarTargetScale.x = 0;
            }
            else
            {
                timerBarTargetScale.x -= timerBarShrinkAmount * Time.deltaTime;
            }
            timerBar.localScale = timerBarTargetScale;
        }
    }

    private void SpinTimerHands()
    {
        if (isTimerRunning)
        {
            timerHands.Rotate(0, 0, 5);
        }
    }
    public void ResetTimer()
    {
        timeRemaining = time;
        timerBarTargetScale = timerBar.localScale;
        timerBarTargetScale.x = timerBarLength;
        timerBar.localScale = timerBarTargetScale;
    }
    public void StartTimer()
    {
        isTimerRunning = true;
    }
    public void StopTimer()
    {
        isTimerRunning = false;
    }
}
