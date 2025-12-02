using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public class moveToBeat : MonoBehaviour
{

    [SerializeField] bpmCounter bpmCounter;
    private float startPosition;
    [SerializeField] private float maxHeight = 3;
    [SerializeField] private int beatOffset = 1;
    [SerializeField] private bool isDoubleTime = false;
    private int bonusBeat;
    private bool isUp = false;
    private bool isMoving;
    private float lastBeatTime;
    private Vector3 targetPosition;
    private Vector3 currentPosition;
    private bool startupPanic = true;
    [SerializeField] private bool isStuck = false;
    [SerializeField] private GameObject doorPistonIsStuckBehind;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position.y;

        lastBeatTime = bpmCounter.GetSongPosition();

        if (isDoubleTime == true)
        {
            bonusBeat = (beatOffset + 2) % 4;
            if (bonusBeat == 0)
            {
                bonusBeat = 4;
            }
        }
        else
        {
            bonusBeat = beatOffset;
        }
        StartCoroutine(WaitForSongPositionFreakOutToEnd());
    }



    // Update is called once per frame
    void Update()
    {
        if (isStuck == true)
        {
            CheckIfUnstuck();
        }
        if (lastBeatTime == 0)
        {
            lastBeatTime = bpmCounter.GetSongPosition();
        }

        CheckCurrentBeat();
    }

    private void CheckCurrentBeat()
    {
        if (bpmCounter.GetSongPosition() < (lastBeatTime + bpmCounter.GetTimeBetweenBeats()))
        {
            return;
        }

        lastBeatTime += bpmCounter.GetTimeBetweenBeats();
        if (isStuck == true)
        {
            return;
        }
        if (bpmCounter.GetCurrentBeat() == beatOffset || bpmCounter.GetCurrentBeat() == bonusBeat)
        {
            isMoving = true;
            SetTargetPosition();
            StartCoroutine(Move());
        }
    }


    private IEnumerator Move()
    {
        if (isMoving == false)
        {
            yield break;
        }
        for (float t = 0; t < 1; t += Time.deltaTime / bpmCounter.GetTimeBetweenBeats())
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, t);
            yield return null;
        }
        isMoving = false;
    }
    //https://stackoverflow.com/a/65814734

    private void SetTargetPosition()
    {
        if (isStuck == true)
        {
            return;
        }
        currentPosition = transform.position;
        targetPosition = new Vector3(transform.position.x, 0, transform.position.z);

        if (isUp)
        {
            targetPosition.y = startPosition;
            isUp = false;
        }
        else
        {
            targetPosition.y = startPosition + maxHeight;
            isUp = true;
        }
    }

    private void CheckIfUnstuck()
    {
        if (doorPistonIsStuckBehind == null)
        {
            isStuck = false;
        }
    }

    private IEnumerator WaitForSongPositionFreakOutToEnd()
    {
        yield return new WaitForSeconds(2);
        lastBeatTime = bpmCounter.GetSongPosition();
        startupPanic = false;
    }
}
