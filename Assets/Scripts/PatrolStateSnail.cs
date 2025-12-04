using UnityEngine;

public class PatrolStateSnail : MonoBehaviour
{
    //https://www.youtube.com/watch?v=hPBkbqqP4m0

    public Rigidbody2D rb;
    public Transform ledgeDetector;
    public LayerMask groundLayer, enemyLayer;

    public AudioSource snailSound;        
    //public float volume = 0.05f;



    
    public float groundDistance, enemyDistance, wallDistance;
    private bool faceRight = false;
    private float bpm;
    private float bpmMove;

    [SerializeField] bpmCounter bpmCounter;
    

    void Start()
    {
        snailSound.volume = 0.05f; 
    }

    private void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, groundDistance, groundLayer);
        RaycastHit2D hitWallL = Physics2D.Raycast(ledgeDetector.position, Vector2.left, wallDistance, groundLayer);
        RaycastHit2D hitWallR = Physics2D.Raycast(ledgeDetector.position, Vector2.right, wallDistance, groundLayer);
        RaycastHit2D hitEnemy = Physics2D.Raycast(ledgeDetector.position, Vector2.left, enemyDistance, enemyLayer);

        bpm = bpmCounter.GetBPM();

        bpmMove = (bpm / 40);

        if (hit.collider == null | hitEnemy.collider == true | hitWallR.collider == true | hitWallL.collider == true)
        {
            if (snailSound != null)
            {
                snailSound.time = 0.1f;
                snailSound.volume = 1;
                snailSound.Play();

            }

            Rotate();

        }
    }

    void FixedUpdate()
    {
        if (faceRight)
        {
            rb.linearVelocity = new Vector2(bpmMove, rb.linearVelocity.y);
            //print("right");
        }
        else
        {
            rb.linearVelocity = new Vector2(-bpmMove, rb.linearVelocity.y);
            //print("left");
        }
        
    }

    void Rotate()
    {
        
            
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
        //print("rot");
        
    
    }
}
