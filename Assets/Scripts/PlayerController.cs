using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum WalkState { Idle, Walking, Running, Jumping, Rolling }

    [SerializeField] private float moveSpeed = 6.5f;
    [SerializeField] private float jumpHeight = 20f;
    [SerializeField] private float rollSpeed = 10f;
    [SerializeField] private float rollDuration = 0.6f;

    private float horizontal;
    private bool isGrounded = true;
    private bool isRolling = false;
    private WalkState walkState = WalkState.Idle;
    private Vector3 directionFacing = Vector3.right;
    private Vector3 lastCheckpointPosition;

    private Transform currentPlatform;
    private Vector3 lastPlatformPos;

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D groundCheckCol;

    private MusicController musicController;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        musicController = GetComponent<MusicController>(); // Reference MusicController
        lastCheckpointPosition = transform.position; // default starting position
        musicController?.StartTracks(); // start music if available
    }

    private void Update()
    {
        musicController?.SetDrumVolume(Mathf.Abs(horizontal));

        if (Input.GetKeyDown(KeyCode.JoystickButton1) && !isRolling && isGrounded)
            StartCoroutine(Roll());
    }

    private void FixedUpdate() => Move();

    public void onMoveInput(float horizontal)
    {
        this.horizontal = horizontal;

        if (Mathf.Abs(horizontal) < 0.8f && horizontal != 0)
        {
            walkState = WalkState.Walking;
            animator.SetInteger("state", 1);
        }
        else if (Mathf.Abs(horizontal) >= 0.8f)
        {
            walkState = WalkState.Running;
            animator.SetInteger("state", 2);
        }
        else
        {
            walkState = WalkState.Idle;
            animator.SetInteger("state", 0);
        }

        // Update facing direction
        if (horizontal != 0)
        {
            directionFacing = new Vector3(Mathf.Sign(horizontal), 0, 0);
            animator.transform.localScale = new Vector3(0.3929782f * Mathf.Sign(horizontal), 0.3929782f, 0.3929782f);
        }
    }

    public void onJumpInput()
    {
        if (isGrounded)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpHeight);
            animator.SetInteger("state", 3);
            musicController?.PlayJumpSound();
        }
    }

    public void onRollInput()
    {
        if (!isRolling && isGrounded)
        {
            musicController?.PlayRollSound();
            StartCoroutine(Roll());
        }
    }

    private void Move()
    {
        Vector3 moveDirection = Vector3.right * horizontal * moveSpeed * Time.deltaTime;

        if (currentPlatform != null)
            transform.localPosition += moveDirection; // localPosition so it moves relative to platform
        else
            transform.position += moveDirection;
    }


    private System.Collections.IEnumerator Roll()
    {
        isRolling = true;
        animator.SetInteger("state", 4);
        float originalSpeed = moveSpeed;
        moveSpeed = rollSpeed;

        yield return new WaitForSeconds(rollDuration);

        moveSpeed = originalSpeed;
        animator.SetInteger("state", 0);
        isRolling = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetInteger("state", 0);
        }

        // Parent to moving platform
        if (collision.CompareTag("MovingPlatform"))
        {
            currentPlatform = collision.transform;
            transform.SetParent(currentPlatform);
            lastPlatformPos = currentPlatform.position;
            isGrounded = true;
        }

        if (collision.CompareTag("Respawn"))
        {
            Vector3 checkpointPos = collision.transform.position;
            lastCheckpointPosition = new Vector3(checkpointPos.x, checkpointPos.y, transform.position.z);
                
            foreach (Checkpoint cp in FindObjectsOfType<Checkpoint>())
                cp.SetInactive();

            collision.GetComponent<Checkpoint>()?.SetActive();
        }

        if (collision.CompareTag("Enemies"))
        {
            transform.position = lastCheckpointPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetInteger("state", 3);
        }
        // Unparent from moving platform
        if (collision.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
            currentPlatform = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
            transform.position = lastCheckpointPosition;
    }
}
