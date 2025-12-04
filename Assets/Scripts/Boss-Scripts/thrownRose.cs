using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class thrownRose : MonoBehaviour
{
    const float THROW_SPEED = 1f;
    [SerializeField] private Vector2 targetPosition;
    private Vector3 targetScale;
    private Vector3 startingScale;
    private GameObject[] roses;
    private bool[] isRoseBeingThrown;
    private float[] xTarget;
    private float[] yTarget;
    private float[] maxThrownHeights;
    private Vector2[] startingPosition;
    private float[] progress;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetChildren();

        isRoseBeingThrown = new bool[roses.Length];
        xTarget = new float[roses.Length];
        yTarget = new float[roses.Length];
        maxThrownHeights = new float[roses.Length];
        startingPosition = new Vector2[roses.Length];
        progress = new float[roses.Length];
        ResetIsRoseBeingThrown();
        RandomizeStartingXPositions();
        ResetRosePositions();

        startingScale = roses[0].transform.localScale;
        targetScale = startingScale * 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveRose();
        ShrinkRoses();
    }

    private void GetChildren()
    {
        roses = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            roses[i] = child.gameObject;
            i++;
        }
    }

    public void ThrowAllRoses()
    {
        Debug.Log("Throwing activated");
        RandomizeMaxThrownHeights();


        int i = 0;
        foreach (GameObject rose in roses)
        {
            Debug.Log("Throwing rose number " + i);
            float randomTime = Random.Range(0.2f, 1f);
            StartCoroutine(ThrowRoseAfterAMoment(randomTime, i));
            i++;
        }
    }


    private IEnumerator ThrowRoseAfterAMoment(float timeUntilThrow, int roseNumber)
    {
        yield return new WaitForSeconds(timeUntilThrow);
        isRoseBeingThrown[roseNumber] = true;
    }

    private void MoveRose()
    {
        for (int i = 0; i < roses.Length; i++)
        {
            if (isRoseBeingThrown[i] == true)
            {
                // Increment our progress from 0 at the start, to 1 when we arrive.
                progress[i] = Mathf.Min(progress[i] + Time.deltaTime * THROW_SPEED, 1.0f);

                // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
                float parabola = 1.0f - 4.0f * (progress[i] - 0.5f) * (progress[i] - 0.5f);

                // Travel in a straight line from our start position to the target.        
                Vector2 nextPos = Vector2.Lerp(startingPosition[i], targetPosition, progress[i]);

                // Then add a vertical arc in excess of this.
                nextPos.y += parabola * maxThrownHeights[i];

                // Continue as before.
                roses[i].transform.position = nextPos;
            }
        }

    }
    // parabola code from 
    // https://gamedev.stackexchange.com/a/183514

    private void ShrinkRoses()
    {
        for (int i = 0; i < roses.Length; i++)
        {
            if (isRoseBeingThrown[i] == true)
            {
                Debug.Log(roses[i].transform.localScale);
                roses[i].transform.localScale = Vector3.MoveTowards(roses[i].transform.localScale, targetScale, 1.5f * Time.deltaTime);
            }
        }
    }
    private void ResetIsRoseBeingThrown()
    {
        for (int i = 0; i < isRoseBeingThrown.Length; i++)
        {
            isRoseBeingThrown[i] = false;
        }
    }


    private void RandomizeMaxThrownHeights()
    {
        for (int i = 0; i < yTarget.Length; i++)
        {
            float randomHeight = Random.Range(3f, 7f);
            maxThrownHeights[i] = targetPosition.y + randomHeight;
        }
    }

    private void RandomizeStartingXPositions()
    {
        for (int i = 0; i < startingPosition.Length; i++)
        {
            float randomPosition = Random.Range(-10.0f, 10.0f);
            startingPosition[i].x = transform.position.x + randomPosition;
            startingPosition[i].y = -5;
        }
    }

    private void ResetRosePositions()
    {
        for (int i = 0; i < startingPosition.Length; i++)
        {
            roses[i].transform.position = startingPosition[i];
        }
    }
}
