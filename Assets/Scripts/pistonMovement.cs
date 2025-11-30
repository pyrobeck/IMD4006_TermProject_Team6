using Unity.VisualScripting;
using UnityEngine;

public class moveToBeat : MonoBehaviour
{
    const int BEATS_BETWEEN_MOVEMENT = 3;
    [SerializeField] bpmCounter bpmCounter;
    private float startPosition;
    [SerializeField] private float maxHeight = 3;
    private float moveSpeed;
    [SerializeField] private int beatOffset = 0;
    private bool isUp = false;
    private bool isMoving;
    private float lastBeatTime;
    private Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position.y;

        moveSpeed = maxHeight / bpmCounter.GetTimeBetweenBeats();
        lastBeatTime = bpmCounter.GetSongPosition();

    }



    // Update is called once per frame
    void Update()
    {
        if (lastBeatTime == 0)
        {
            lastBeatTime = bpmCounter.GetSongPosition();
        }

        CheckCurrentBeat();

        Move();
    }

    private void CheckCurrentBeat()
    {
        if (bpmCounter.GetSongPosition() < (lastBeatTime + bpmCounter.GetTimeBetweenBeats()))
        {
            return;
        }

        lastBeatTime += bpmCounter.GetTimeBetweenBeats();
        if (bpmCounter.GetCurrentBeat() == 4)
        {
            isMoving = true;
            SetTargetPosition();
        }
    }

    private void Move()
    {
        if (isMoving == false)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxHeight / moveSpeed);

        if (transform.position.y == targetPosition.y)
        {
            isMoving = false;
        }
    }

    private void SetTargetPosition()
    {
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
}
