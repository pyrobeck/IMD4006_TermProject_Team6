using UnityEngine;
using System.Collections;

public class changeAnimationEveryBeat : MonoBehaviour
{
    [SerializeField] private bpmCounter bpmCounter;
    [SerializeField] private int numberOfFrames = 2;
    [SerializeField] private bool isHalfSpeed = false;
    [SerializeField] private bool isQuarterSpeed = false;
    [SerializeField] float offset = 0;
    private Animator animator;
    private int currentFrame = 0;
    private bool startUpPanic = true;

    private float lastBeatTime;
    void Start()
    {
        animator = GetComponent<Animator>();

        lastBeatTime = bpmCounter.GetSongPosition();
        StartCoroutine(WaitForSongPositionFreakOutToEnd());
    }

    // Update is called once per frame
    void Update()
    {
        if (startUpPanic == true)
        {
            return;
        }
        UpdateCurrentBeat();
    }

    private void UpdateCurrentBeat()
    {
        if (bpmCounter.GetSongPosition() < (lastBeatTime + bpmCounter.GetTimeBetweenBeats() - offset))
        {
            return;
        }

        lastBeatTime += bpmCounter.GetTimeBetweenBeats();
        CheckCorrectNumberOfBeats();
    }



    private void CheckCorrectNumberOfBeats()
    {
        CheckCurrentBeat(1);
        if (isQuarterSpeed == true)
        {
            return;
        }
        CheckCurrentBeat(3);
        if (isHalfSpeed == true)
        {
            return;
        }

        CheckCurrentBeat(2);
        CheckCurrentBeat(4);

    }

    private void CheckCurrentBeat(int beatNumber)
    {

        if (bpmCounter.GetCurrentBeat() == beatNumber)
        {
            animator.SetInteger("state", currentFrame);
            currentFrame++;
            if (currentFrame > numberOfFrames - 1)
            {
                currentFrame = 0;
            }
        }
    }

    private IEnumerator WaitForSongPositionFreakOutToEnd()
    {
        yield return new WaitForSeconds(2);
        lastBeatTime = bpmCounter.GetSongPosition();
        startUpPanic = false;
    }
}
