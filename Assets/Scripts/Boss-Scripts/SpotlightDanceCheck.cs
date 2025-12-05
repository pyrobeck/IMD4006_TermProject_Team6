using UnityEngine;
using UnityEngine.Events;
public class SpotlightDanceCheck : MonoBehaviour
{
    private bool isPlayerUnderSpotlight = false;
    public UnityEvent danceSuccessful;
    public UnityEvent StartBoss;
    private bool isUsed = false;
    private bool isBossStarted = false;

    [SerializeField] Vector2[] targetPositions;
    int currentLocation = 1;

    void Start()
    {
        transform.position = targetPositions[0];
    }
    public void OnDanceInput(Vector2 stickInput)
    {
        if (isUsed)
        {
            return;
        }
        if (isPlayerUnderSpotlight)
        {
            if (isBossStarted == false)
            {
                isBossStarted = true;
                StartBoss.Invoke();
            }
            danceSuccessful.Invoke();
            isUsed = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isUsed)
        {
            return;
        }
        // Debug.Log("something entered colission");
        if (collider.gameObject.CompareTag("Player"))
        {
            // Debug.Log("It was the player!!!");
            isPlayerUnderSpotlight = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (isUsed)
        {
            return;
        }
        if (collider.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Bye player!!!");
            isPlayerUnderSpotlight = false;
        }
    }

    public void Move()
    {
        isUsed = false;
        isPlayerUnderSpotlight = false;
        transform.position = targetPositions[currentLocation];
        currentLocation++;
    }

    public void DisableSpotlight()
    {
        isUsed = true;
    }
}
