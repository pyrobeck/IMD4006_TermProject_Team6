using UnityEngine;
using UnityEngine.Events;
public class SpotlightDanceCheck : MonoBehaviour
{
    private bool isPlayerUnderSpotlight = false;
    public UnityEvent danceSuccessful;
    public void OnDanceInput(Vector2 stickInput)
    {
        if (isPlayerUnderSpotlight)
        {
            // Debug.Log("WE'RE DANCING!!!! UNDER THE SPOT LIGHT!!!");
            danceSuccessful.Invoke();
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Debug.Log("something entered colission");
        if (collider.gameObject.CompareTag("Player"))
        {
            // Debug.Log("It was the player!!!");
            isPlayerUnderSpotlight = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        //Debug.Log("something exited colission");
        if (collider.gameObject.CompareTag("Player"))
        {
            //  Debug.Log("Bye player!!!");
            isPlayerUnderSpotlight = false;
        }
    }
}
