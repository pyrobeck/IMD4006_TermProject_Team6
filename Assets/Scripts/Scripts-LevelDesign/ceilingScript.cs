using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Rigidbody2D rb;           // Platform Rigidbody
    public Transform upTrigger;      // The object that triggers moving up
    public float downSpeed = 2f;
    public float upSpeed = 5f;

    private Vector2 initialPos;
    private bool moveDown = false;
    private bool moveUp = false;

    void Start()
    {
        if (rb == null || upTrigger == null)
        {
            Debug.LogError("Assign platform Rigidbody and upTrigger!");
            enabled = false;
            return;
        }

        initialPos = rb.position;
    }

    void FixedUpdate()
    {
        if (moveDown)
        {
            rb.MovePosition(rb.position + Vector2.down * downSpeed * Time.fixedDeltaTime);
        }
        else if (moveUp)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, initialPos, upSpeed * Time.fixedDeltaTime));
        }
    }

    public void StartMovingDown()
    {
        moveDown = true;
        moveUp = false;
        Debug.Log("Platform moving down.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detect entering the up trigger
        if (other.transform == upTrigger)
        {
            moveDown = false;
            moveUp = true;
            Debug.Log("Platform entered up trigger: moving up.");
        }
    }

}
