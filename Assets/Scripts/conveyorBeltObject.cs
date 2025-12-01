using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class conveyorBeltObject : MonoBehaviour
{
    private enum State
    {
        Spawning, //0
        Stationary, //1
        Moving,
        Falling,
        AboutToSpawn,
        AboutToFall

    }
    const float DISTANCE_BETWEEN_SOUCE_AND_BELT = 1.5f;
    [SerializeField] bpmCounter bpmCounter;
    [SerializeField] int speed; //1, 2, or 4
    [SerializeField] Transform sourcePosition;
    [SerializeField] private int moves; //amount of times it moves forward on the conveyor belt
    [SerializeField] private int startPosition = 0; //its starting position in a lineup of objects on a conveyor belt
    [SerializeField] private float distanceTravelledPerBeat;
    private float lastBeatTime;
    private Vector3 targetPosition;
    private Vector3 currentPosition;
    private State state = State.Stationary;
    private int remainingMoves;
    private bool startupPanic = true;
    private float speedTimingFactor = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (sourcePosition == null)
        {
            Debug.Log("Connect the object source!!!!!!!!");
        }
        if (startPosition > moves)
        {
            Debug.Log("attempting to start further than it can move");
            startPosition = moves;
        }

        lastBeatTime = bpmCounter.GetSongPosition();
        remainingMoves = moves - startPosition;
        if (speed == 4)
        {
            speedTimingFactor = 2;
        }

        SetStartPosition();
        StartCoroutine(WaitForSongPositionFreakOutToEnd());
    }



    // Update is called once per frame
    void Update()
    {
        if (startupPanic == true)
        {
            return;
        }
        if (lastBeatTime == 0)
        {
            lastBeatTime = bpmCounter.GetSongPosition();
        }

        UpdateCurrentBeat();


    }

    private void CheckCurrentBeat(int beatNumber)
    {

        if (bpmCounter.GetCurrentBeat() == beatNumber)
        {
            switch (state)
            {
                case State.Stationary:
                    state = State.Moving;
                    break;
                case State.AboutToSpawn:
                    state = State.Spawning;
                    break;
                case State.AboutToFall:
                    state = State.Falling;
                    break;
                default:
                    state = State.Moving;
                    break;
            }

            SetTargetPosition();
            StartCoroutine(Move());
        }
    }

    private void CheckCorrectNumberOfBeats()
    {
        if (speed == 4)
        {
            CheckCurrentBeat(4);
            CheckCurrentBeat(2);
        }
        if (speed >= 2)
        {
            CheckCurrentBeat(3);
        }
        if (speed >= 1)
        {
            CheckCurrentBeat(1);
        }
    }

    private void UpdateCurrentBeat()
    {
        if (bpmCounter.GetSongPosition() < (lastBeatTime + bpmCounter.GetTimeBetweenBeats()))
        {
            return;
        }

        lastBeatTime += bpmCounter.GetTimeBetweenBeats();
        CheckCorrectNumberOfBeats();
    }
    private IEnumerator Move()
    {
        if (state == State.Stationary)
        {
            yield break;
        }
        for (float t = 0; t < 1; t += Time.deltaTime / (bpmCounter.GetTimeBetweenBeats() / speedTimingFactor))
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, t);
            yield return null;
        }
        if (state == State.Falling)
        {
            state = State.AboutToSpawn;
            remainingMoves = moves;
            ResetPositionToSource();
        }
        else
        {
            state = State.Stationary;
            remainingMoves -= 1;
        }
        if (remainingMoves <= 0)
        {
            state = State.AboutToFall;
        }

    }
    //https://stackoverflow.com/a/65814734

    private void SetTargetPosition()
    {
        currentPosition = transform.position;
        targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        switch (state)
        {
            case State.Moving:
                targetPosition.x = transform.position.x + distanceTravelledPerBeat;
                break;
            case State.Falling:
                targetPosition.y = transform.position.y - transform.GetChild(0).transform.localScale.y;
                break;
            case State.Spawning:
                targetPosition.y = transform.position.y + transform.GetChild(0).transform.localScale.y + DISTANCE_BETWEEN_SOUCE_AND_BELT;
                break;
            default:
                targetPosition.x = transform.position.x;
                break;
        }
    }

    private void ResetPositionToSource()
    {
        Vector3 newPosition = sourcePosition.position;
        newPosition.y -= transform.GetChild(0).transform.localScale.y / 2;
        transform.position = newPosition;
    }

    private void SetStartPosition()
    {
        if (startPosition == 0)
        {
            ResetPositionToSource();
            state = State.AboutToSpawn;
            return;
        }
        if (startPosition == moves)
        {
            state = State.AboutToFall;
        }
        Vector3 startPos = new Vector3(0, 0, transform.position.z);
        startPos.y = sourcePosition.position.y + transform.GetChild(0).transform.localScale.y;
        startPos.x = sourcePosition.position.x + (distanceTravelledPerBeat * (startPosition - 1));

        transform.position = startPos;
    }

    private IEnumerator WaitForSongPositionFreakOutToEnd()
    {
        yield return new WaitForSeconds(2);
        lastBeatTime = bpmCounter.GetSongPosition();
        startupPanic = false;
    }
}
