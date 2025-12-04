using UnityEngine;
using UnityEngine.Events;
public class SpotlightDanceCheck : MonoBehaviour
{
    private bool isPlayerUnderSpotlight = false;
    public UnityEvent danceSuccessful;
    private bool isUsed = false;

    [SerializeField] Vector2[] targetPositions;
    int currentLocation = 0;
    public void OnDanceInput(Vector2 stickInput)
    {
        if (isUsed)
        {
            return;
        }
        if (isPlayerUnderSpotlight)
        {
            // Debug.Log("WE'RE DANCING!!!! UNDER THE SPOT LIGHT!!!");
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
            //  Debug.Log("Bye player!!!");
            isPlayerUnderSpotlight = false;
        }
    }

    public void Move()
    {
        isUsed = false;
        currentLocation++;
        transform.position = targetPositions[currentLocation];
    }
}
