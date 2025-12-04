using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;



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
    private float bpm;
    private float bpmMove;

    [SerializeField] bpmCounter bpmCounter;

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

        bpm = bpmCounter.GetBPM();

        bpmMove = (bpm / 20);

        //print("current " + currentPos.y);
        //print("start " + startPos.y);

        if (hitPlayer.collider == true)
        {
            if (audioSource != null)
                audioSource.PlayOneShot(Hit, 0.25f);

            

            //rb.linearVelocity = new Vector2(bpm, rb.linearVelocity.x);
            StartCoroutine(waitDown());
            

        }

        if (hitFloor.collider == true)
        {
            if (audioSource != null)
                audioSource.PlayOneShot(Fall, 0.5f);

            //print("floor");
            StartCoroutine(waitUp());

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

        rb.linearVelocity = new Vector2(0, -bpmMove);

    }

    void moveUp()
    {


        rb.linearVelocity = new Vector2(0, (bpmMove / 2));
        stop = true;

    }

    private IEnumerator waitDown()
    {
        //shake
        transform.rotation = Quaternion.Euler(0, 0, 10);
        yield return new WaitForSeconds(0.05f);

        transform.rotation = Quaternion.Euler(0, 0, -10);
        yield return new WaitForSeconds(0.05f);

        transform.rotation = Quaternion.Euler(0, 0, 10);
        yield return new WaitForSeconds(0.05f);

        transform.rotation = Quaternion.Euler(0, 0, -10);
        yield return new WaitForSeconds(0.05f);

        transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(0.05f);

        //yield return new WaitForSeconds(0.25f);

        moveDown();

    }

    private IEnumerator waitUp()
    {
        yield return new WaitForSeconds(0.5f);

        moveUp();

    }


}
