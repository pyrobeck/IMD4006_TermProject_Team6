using UnityEngine;
using UnityEngine.Events;

public class BossTimer : MonoBehaviour
{
    const float PROPER_POSITION_X = -14.77f;
    const float PROPER_POSITION_Y = 7.38f;
    const float PROPER_POSITION_Z = 1.5f;
    [SerializeField] private float time;
    [SerializeField] private Transform timerHands;
    public UnityEvent Failure;
    private float timeRemaining;
    private Transform timerBar;
    bool isTimerRunning = false;
    private float timerBarLength;
    private float timerBarShrinkAmount;
    private Vector3 timerBarTargetScale;

    private bool isBossStarted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerBar = transform.GetChild(0);
        timerBarLength = timerBar.localScale.x;
        timerBarShrinkAmount = timerBarLength / time;
        timerBarTargetScale = timerBar.localScale;
        ResetTimer();
        transform.position = Vector2.up * 99999;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBossStarted == false)
        {
            return;
        }
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
            timerHands.Rotate(0, 0, -5);
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
    public void Appear()
    {
        if (isBossStarted == false)
        {
            isBossStarted = true;
            transform.localPosition = new Vector3(PROPER_POSITION_X, PROPER_POSITION_Y, PROPER_POSITION_Z);
            return;
        }
    }
}
