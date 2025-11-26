using UnityEngine;



public class PatrolStateBeatle : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform PlayerDetector, FloorDetector;
    public LayerMask groundLayer, playerLayer;

    public AudioSource audioSource;
    public AudioClip Fall;
    public AudioClip Hit;

    Vector2 startPos;
    Vector2 currentPos;

    bool stop = false;

    public float playerDistance, floorDistance;
    public float bpm;

    void Start()
    {
      //  print(gameObject.name);
        startPos = gameObject.transform.position;
    }


    void Update()
    {
        RaycastHit2D hitPlayer = Physics2D.Raycast(PlayerDetector.position, Vector2.down, playerDistance, playerLayer);
        RaycastHit2D hitFloor = Physics2D.Raycast(FloorDetector.position, Vector2.down, floorDistance, groundLayer);

        currentPos = gameObject.transform.position;

        //print("current " + currentPos.y);
        //print("start " + startPos.y);

        if (hitPlayer.collider == true)
        {
            print("player");
            rb.linearVelocity = new Vector2(bpm, rb.linearVelocity.x);
            moveDown();
            if (audioSource != null)
            audioSource.PlayOneShot(Hit, 0.05f);

        }

        if (hitFloor.collider == true)
        {
            //print("floor");
            moveUp();
        }

        if (currentPos.y > startPos.y && stop == true)
        {
            rb.linearVelocity = new Vector2(0, 0);
            currentPos.y = (startPos.y-1);
            stop = false;
        }

    }

    void moveDown()
    {
        rb.linearVelocity = new Vector2(0, -bpm);
        if (audioSource != null)
            audioSource.PlayOneShot(Fall, 0.05f);
    }

    void moveUp()
    {

        rb.linearVelocity = new Vector2(0, (bpm/2));
        stop = true;
            
    }
}
