using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
public class Tomatoes : MonoBehaviour
{
    const float THROW_SPEED = 1f;
    [SerializeField] private GameObject player;
    private Vector3[] targetPositions;
    private int currentSection = -1;
    private Vector3 targetScale;
    private Vector3 startingScale;
    private GameObject[] tomatoes;
    private bool[] isTomatoBeingThrown;
    private float[] xTarget;
    private float[] yTarget;
    private float[] maxThrownHeights;
    private Vector2[] startingPosition;
    private float[] progress;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetChildren();

        isTomatoBeingThrown = new bool[tomatoes.Length];
        xTarget = new float[tomatoes.Length];
        yTarget = new float[tomatoes.Length];
        maxThrownHeights = new float[tomatoes.Length];
        startingPosition = new Vector2[tomatoes.Length];
        progress = new float[tomatoes.Length];
        targetPositions = new Vector3[tomatoes.Length];

        startingScale = tomatoes[0].transform.localScale;
        targetScale = startingScale * 0.3f;

        ResetTomatoes();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTomato();
        ShrinkTomatoes();
    }

    private void SetTargetPosition(int tomatoNumber)
    {
        targetPositions[tomatoNumber] = player.transform.position;
    }

    private IEnumerator KillPlayer()
    {
        yield return new WaitForSeconds(3);
        player.GetComponent<PlayerController>().KillPlayer();
    }
    private void GetChildren()
    {
        tomatoes = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            tomatoes[i] = child.gameObject;
            i++;
        }
    }

    public void ThrowAllTomatoes()
    {
        StartCoroutine(KillPlayer());
        int i = 0;
        foreach (GameObject tomato in tomatoes)
        {
            float randomTime = Random.Range(0.5f, 2f);
            StartCoroutine(ThrowTomatoAfterAMoment(randomTime, i));
            i++;
        }
    }


    private IEnumerator ThrowTomatoAfterAMoment(float timeUntilThrow, int tomatoNumber)
    {
        yield return new WaitForSeconds(timeUntilThrow);
        SetTargetPosition(tomatoNumber);
        RandomizeMaxThrownHeight(tomatoNumber);
        isTomatoBeingThrown[tomatoNumber] = true;
    }

    private void MoveTomato()
    {
        for (int i = 0; i < tomatoes.Length; i++)
        {
            if (isTomatoBeingThrown[i] == true)
            {
                // Increment our progress from 0 at the start, to 1 when we arrive.
                progress[i] = Mathf.Min(progress[i] + Time.deltaTime * THROW_SPEED, 1.0f);

                // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
                float parabola = 1.0f - 4.0f * (progress[i] - 0.5f) * (progress[i] - 0.5f);

                // Travel in a straight line from our start position to the target.        
                Vector3 nextPos = Vector3.Lerp(startingPosition[i], targetPositions[i], progress[i]);

                // Then add a vertical arc in excess of this.
                nextPos.y += parabola * maxThrownHeights[i];

                // Continue as before.
                tomatoes[i].transform.position = nextPos;
            }
        }

    }
    // parabola code from 
    // https://gamedev.stackexchange.com/a/183514

    private void ShrinkTomatoes()
    {
        for (int i = 0; i < tomatoes.Length; i++)
        {
            if (isTomatoBeingThrown[i] == true)
            {
                tomatoes[i].transform.localScale = Vector3.MoveTowards(tomatoes[i].transform.localScale, targetScale, 1.5f * Time.deltaTime);
            }
        }
    }
    private void ResetIsTomatoBeingThrown()
    {
        for (int i = 0; i < isTomatoBeingThrown.Length; i++)
        {
            isTomatoBeingThrown[i] = false;
        }
    }


    private void RandomizeMaxThrownHeight(int tomatoNumber)
    {
        float randomHeight = Random.Range(3f, 7f);
        maxThrownHeights[tomatoNumber] = targetPositions[tomatoNumber].y + randomHeight;
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

    private void ResetTomatoPositions()
    {
        for (int i = 0; i < startingPosition.Length; i++)
        {
            tomatoes[i].transform.position = startingPosition[i];
        }
    }

    private void ResetTomatoSizes()
    {
        for (int i = 0; i < startingPosition.Length; i++)
        {
            tomatoes[i].transform.localScale = startingScale;
        }
    }

    private void ResetParabolaProgress()
    {
        for (int i = 0; i < progress.Length; i++)
        {
            progress[i] = 0;
        }
    }
    public void ResetTomatoes()
    {
        currentSection++;
        ResetIsTomatoBeingThrown();
        RandomizeStartingXPositions();
        ResetTomatoPositions();
        ResetTomatoSizes();
        ResetParabolaProgress();
    }
}