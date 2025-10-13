using UnityEngine;

public class PatrolStateSnail : MonoBehaviour
{
    //https://www.youtube.com/watch?v=hPBkbqqP4m0

    public Rigidbody2D rb;
    public Transform ledgeDetector;
    public LayerMask groundLayer, enemyLayer;

    
    public float raycastDistance, enemyDistance;
    public float speed;
    private bool faceRight = true;

    void Start()
    {

    }

    private void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, raycastDistance, groundLayer);
        RaycastHit2D hitEnemy = Physics2D.Raycast(ledgeDetector.position, Vector2.right, enemyDistance, enemyLayer);

        if (hit.collider == null | hitEnemy.collider == true)
        {
            Rotate();
        }
    }

    void FixedUpdate()
    {
        if (faceRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
        }
        
    }

    void Rotate()
    {
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
        
    }
}
