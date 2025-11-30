using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public class moveToBeat : MonoBehaviour
{
    const int BEATS_BETWEEN_MOVEMENT = 3;
    [SerializeField] bpmCounter bpmCounter;
    private float startPosition;
    [SerializeField] private float maxHeight = 3;
    private float moveIncrements = 5;
    [SerializeField] private int beatOffset = 0;
    private bool isUp = false;
    private bool isMoving;
    private float lastBeatTime;
    private Vector3 targetPosition;
    private Vector3 currentPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position.y;

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
}
