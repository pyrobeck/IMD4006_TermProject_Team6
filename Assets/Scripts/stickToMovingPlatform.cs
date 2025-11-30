using UnityEngine;

public class stickPlayerToPlatform : MonoBehaviour
{
    private Transform currentPlatform;

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if touching platform
        // var platform = collision.gameObject.GetComponent<stickPlayerToPlatform>();
        // if (platform != null && IsGrounded())
        // {
        //     currentPlatform = platform;
        // }

        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            currentPlatform = collision.gameObject.transform.parent.gameObject.transform; //the parent of the platform they're colliding with lol (to fix scaling issues with parenting)
            if (currentPlatform == null)
            {
                return;
            }
            this.gameObject.transform.SetParent(currentPlatform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            currentPlatform = null;
            this.gameObject.transform.SetParent(null);
        }
    }

}
