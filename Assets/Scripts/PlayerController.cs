using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    private enum WalkState
    {
        Idle, //0
        Walking, //1
        Running, //2
        Jumping, //3
        Rolling, //4
        WallSliding, //5
    }


    [SerializeField] private float moveSpeed = 6.5F;
    [SerializeField] private float jumpHeight = 20F;
    [SerializeField] private float wallJumpDistance = 5f;
    [SerializeField] private float jumpUpwardsGravity = 5f;
    [SerializeField] private float fallingGravity = 7f;
    [SerializeField] private float wallSlidingGravity = 1.5f;
    private bool isJumping = false;
    private bool isWallJumping = false;
    [SerializeField] float coyoteTime = 0.175f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferTimer = 0;
    private float wallJumpTimer = 0;
    private float maxWallJumpTime = 0.5f;


    [SerializeField] private float rollSpeed = 10F;
    [SerializeField] private float rollDuration = 0.6F;

    float horizontal;

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private CapsuleCollider2D collidor;
    [SerializeField] private Animator animator;

    public AudioSource audioSource;
    public AudioSource audioSourceMusic;
    public AudioSource audioSourceBass;
    public AudioSource audioSourceDrums;

    public AudioClip music;
    public AudioClip bass;
    public AudioClip drums;

    public AudioClip jumpAudio;
    public AudioClip rollAudio;
    public float volume = 1.0f;

    [SerializeField] private float minX = -50f;
    [SerializeField] private float maxX = 350f;
    [SerializeField] private float minVolume = 0f;
    [SerializeField] private float maxVolume = 0.8f;

    private PlatformMoveWithBPMBounce currentPlatform;



    public LayerMask groundLayer;

    [SerializeField] public cameraMovement camera;

    [SerializeField] private GameObject heldObjectPrefab;
    [SerializeField] private GameObject thrownObjectPrefab;
    private bool isHoldingObject = false;
    private bool isNearJunkPile = false;
    private bool isThrowingObject = false;
    private float throwCooldownTimer = 0;
    private float maxThrowCooldownTimer = 0.5f;


    private bool isRolling = false;
    WalkState walkState = WalkState.Idle;
    Vector3 directionFacing = new Vector3(1, 0, 0);
    Vector3 spriteScale = new Vector3(0.3929782f, 0.3929782f, 0.3929782f);
    Vector3 spriteScaleFlipped = new Vector3(-0.3929782f, 0.3929782f, 0.3929782f);


    //idle = 0 walking between -1 and 1 running = 1

    private Vector3 lastCheckpointPosition;


    SpriteRenderer temp;

    private void Start()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
        collidor = this.GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Vector3 lastCheckpointPosition = new Vector3(1f, 1f, 1f); // Default position
        coyoteTimeCounter = coyoteTime;

        temp = this.GetComponent<SpriteRenderer>();

        StartTracks();

    }

    private void Update()
    {

        UpdateTimers();
        UpdateBassVolume();
        UpdateDrumTrack();

    }

    private void FixedUpdate()
    {
        Move();
        if (IsGrounded() == false)
        {
            JumpMidairPhysics();
        }

    }

    private void LateUpdate()
    {
        if (currentPlatform != null)
        {
            transform.position += currentPlatform.DeltaPosition;
        }
    }

    public void onMoveInput(float horizontal)
    {
        this.horizontal = horizontal;

        SetDirection();
        SetWalkState();

    }

    public void onJumpInput()
    {

        if (IsNextToWall() && IsGrounded() == false)
        {
            WallJump();
            return;
        }
        //if player is no longer within the grace period of jumping, store the jump input for a moment to see if they hit the ground
        if (coyoteTimeCounter <= 0)
        {
            jumpBuffer();
            coyoteTimeCounter = -1;
            return;
        }

        Jump();
        coyoteTimeCounter = -1; //just in case. I don't trust it.
    }

    public void onJumpCanceled()
    {
        //cuts the vertical velocity when they let go of the jump button to shorten the jump
        rigidBody.linearVelocityY = rigidBody.linearVelocityY * 0.3f;
        isJumping = false;
    }

    public void OnPickupPerformed()
    {
        if (isThrowingObject == false && isHoldingObject == false && isNearJunkPile == true)
        {
            PickUpObject();
            isHoldingObject = true;
        }
    }

    public void OnPickupCancelled()
    {
        if (isHoldingObject == true)
        {
            ThrowObject();
            isHoldingObject = false;
            isThrowingObject = true;
        }
    }

    private void PickUpObject()
    {
        Instantiate(heldObjectPrefab, (transform.position + directionFacing), Quaternion.identity, this.transform);
    }

    private void ThrowObject()
    {
        ClearHeldObjectsFromChildren();
        Instantiate(thrownObjectPrefab, (transform.position + directionFacing), Quaternion.identity, this.transform);
    }

    private void ClearHeldObjectsFromChildren()
    {
        GameObject[] children = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            children[i] = child.gameObject;
            i++;
        }
        i = 0;
        foreach (GameObject child in children)
        {
            if (transform.GetChild(i).CompareTag("HeldObject"))
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            i++;
        }
    }
    // used this code as a refrence https://stackoverflow.com/a/46359133



    private void Jump()
    {
        isJumping = true;
        rigidBody.linearVelocityY = jumpHeight;
        coyoteTimeCounter = -1;
        animator.SetInteger("state", 3);
        PlayJumpSound();
    }

    private void WallJump()
    {
        //adds a force away from the wall they're jumping from
        rigidBody.linearVelocityX = wallJumpDistance * -directionFacing.x;
        isWallJumping = true;
        ResetWallJumpTimer();
        ReverseDirection();
        Jump();
    }

    private void JumpMidairPhysics()
    {
        //if the player is moving upwards
        if (rigidBody.linearVelocityY > 0)
        {
            rigidBody.gravityScale = jumpUpwardsGravity;
            return;
        }

        if (IsWallSliding() == true)
        {
            rigidBody.gravityScale = wallSlidingGravity;
            return;
        }

        if (rigidBody.linearVelocityY <= 0)
        {
            rigidBody.gravityScale = fallingGravity;
            return;
        }
    }

    private void jumpBuffer()
    {
        jumpBufferTimer = jumpBufferTime;
    }
    //when jump input is given while in the air, start a short timer 
    //if the player lands on the ground in that time, input the jump

    public void onRollInput()
    {
        //Roll code goes in here
        //Remember to connect the player and their functions to
        //the input controller script on the game manager
        if (!isRolling && IsGrounded())
        {
            PlayRollSound();
            StartCoroutine(Roll());
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Store the position of the checkpoint
        if (collision.CompareTag("Respawn"))
        {
            lastCheckpointPosition = new Vector3(collision.transform.position.x, collision.transform.position.y, transform.position.z);
            Debug.Log("Checkpoint reached at: " + lastCheckpointPosition);

            foreach (Checkpoint cp in FindObjectsOfType<Checkpoint>())
                cp.SetInactive();

            Checkpoint thisCheckpoint = collision.GetComponent<Checkpoint>();
            if (thisCheckpoint != null)
                thisCheckpoint.SetActive();
        }

        if (collision.CompareTag("Enemies"))
        {
            transform.position = lastCheckpointPosition;
            camera.SnapToTarget();
            Debug.Log("Hit enemy! Respawning at: " + lastCheckpointPosition);
        }

        if (collision.gameObject.CompareTag("JunkPile"))
        {
            isNearJunkPile = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("JunkPile"))
        {
            isNearJunkPile = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
        {
            transform.position = lastCheckpointPosition;
            camera.SnapToTarget();
        }

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if touching platform
        var platform = collision.gameObject.GetComponent<PlatformMoveWithBPMBounce>();
        if (platform != null && IsGrounded())
        {
            currentPlatform = platform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlatformMoveWithBPMBounce>() != null)
        {
            currentPlatform = null;
        }
    }

    private bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 size = collidor.bounds.size * 0.7f;
        float angle = 0;
        Vector2 direction = Vector2.down;
        float distance = 0.5f;

        RaycastHit2D groundCheck = Physics2D.BoxCast(position, size, angle, direction, distance, groundLayer);

        if (!groundCheck)
        {
            return false;
        }

        if (groundCheck.collider.CompareTag("Baseline"))
        {
            camera.SetNewCameraBaseline();
        }

        return true;
    }

    private bool IsNextToWall()
    {
        Vector2 position = transform.position;
        Vector2 size = collidor.bounds.size * 0.7f;
        float angle = 0;
        Vector2 direction = directionFacing;
        float distance = 0.5f;

        RaycastHit2D wallCheck = Physics2D.BoxCast(position, size, angle, direction, distance, groundLayer);

        if (wallCheck)
        {
            walkState = WalkState.WallSliding;
            return true;
        }
        return false;
    }


    private bool IsWallSliding()
    {
        if (IsNextToWall() && IsGrounded() == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void Move()
    {
        //avoiding the weird movement when the player is rolling and moving
        if (isRolling) return;

        if (wallJumpTimer > 0 && IsInputDirectionSameAsDirectionFacing() == false)
        {
            return;
        }

        Vector3 moveDirection = Vector3.right * horizontal;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void ResetWallJumpTimer()
    {
        wallJumpTimer = maxWallJumpTime;
    }
    private void SetDirection()
    {
        if (horizontal == 0)
        {
            return;
        }
        if (horizontal > 0)
        {
            directionFacing = new Vector3(1, 0, 0);
            animator.transform.localScale = spriteScale;
        }
        if (horizontal < 0)
        {
            directionFacing = new Vector3(-1, 0, 0);
            animator.transform.localScale = spriteScaleFlipped;
        }
    }
    private void SetDirection(int direction)
    {
        if (direction == 1)
        {
            directionFacing = new Vector3(1, 0, 0);
            animator.transform.localScale = spriteScale;
            return;
        }
        if (direction == -1)
        {
            directionFacing = new Vector3(-1, 0, 0);
            animator.transform.localScale = spriteScaleFlipped;
            return;
        }
    }

    private void ReverseDirection()
    {
        if (directionFacing.x == 1)
        {
            SetDirection(-1);
            return;
        }
        if (directionFacing.x == -1)
        {
            SetDirection(1);
            return;
        }
    }

    private bool IsInputDirectionSameAsDirectionFacing()
    {
        if (horizontal > 0 && directionFacing.x == 1)
        {
            return true;
        }
        if (horizontal < 0 && directionFacing.x == -1)
        {
            return true;
        }
        return false;
    }

    private void SetWalkState()
    {
        if (horizontal == 0)
        {
            walkState = WalkState.Idle;
            animator.SetInteger("state", 0);
            return;
        }
        if (horizontal < 0.8 && horizontal > -0.8)
        {
            walkState = WalkState.Walking;
            animator.SetInteger("state", 1);
            return;
        }
        if (horizontal >= 1 || horizontal <= -1) //help I don't know the absolute function in C#
        {
            walkState = WalkState.Running;
            animator.SetInteger("state", 2);
            return;
        }
    }
    private System.Collections.IEnumerator Roll()
    {
        // Debug.Log("Rolling!");
        isRolling = true;
        animator.SetInteger("state", 4);
        PlayRollSound();

        //in case we want the roll to be smoother
        //rigidBody.linearVelocity = new Vector2(0, 0);

        //pushing the roll once
        rigidBody.AddForce(directionFacing * rollSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(rollDuration);

        isRolling = false;
        animator.SetInteger("state", 0);
    }

    /// Timers
    private void UpdateTimers()
    {
        coyoteTimer();
        updateJumpBufferTimer();
        WallJumpTimer();
        ThrowCooldownTimer();
    }

    private void coyoteTimer()
    {
        //starts a timer whenever the player leaves the ground. Resets it once they return to ground
        if (IsGrounded() && isJumping == false)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    private void updateJumpBufferTimer()
    {
        jumpBufferTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0 && IsGrounded() == true)
        {
            onJumpInput();
            jumpBufferTimer = -1;
        }
    }
    private void WallJumpTimer()
    {
        //starts a timer whenever the player leaves the ground. Resets it once they return to ground
        if (isWallJumping == true)
        {
            wallJumpTimer -= Time.deltaTime;
        }

        if (wallJumpTimer <= 0)
        {
            isWallJumping = false;
        }
    }

    private void ThrowCooldownTimer()
    {
        if (isThrowingObject == true)
        {
            throwCooldownTimer -= Time.deltaTime;
        }
        else
        {
            throwCooldownTimer = maxThrowCooldownTimer;
        }
        if (throwCooldownTimer <= 0)
        {
            isThrowingObject = false;
        }
    }
    ///////////////////// Play Music ///////////////////////////////////////////////////////
    public void StartTracks()
    {
        audioSourceMusic.clip = music;
        audioSourceBass.clip = bass;
        audioSourceDrums.clip = drums;

        // Start background tracks
        audioSourceMusic.clip = music;
        audioSourceMusic.volume = 0.5f; // half volume
        audioSourceMusic.loop = true;
        audioSourceMusic.Play();

        audioSourceBass.clip = bass;
        audioSourceBass.volume = 0.0f;// start silent
        audioSourceBass.loop = true;
        audioSourceBass.Play();

        audioSourceDrums.clip = drums; //Horizontal movement
        audioSourceDrums.volume = 1f; // start silent
        audioSourceDrums.loop = true;
        audioSourceDrums.Play();
    }

    public void drumVol()
    {
        audioSourceDrums.volume = Mathf.Abs(horizontal);
    }

    public void PlayJumpSound()
    {
        //Debug.Log("Enters walksounds function");
        audioSource.PlayOneShot(jumpAudio, 1.0f);

    }

    public void PlayRollSound()
    {
        Debug.Log("Enters rollsounds function");
        audioSource.PlayOneShot(rollAudio, 1.0f);

    }

    private void UpdateBassVolume()
    {
        float xPos = transform.position.x;

        // Clamp X between minX and maxX
        float clampedX = Mathf.Clamp(xPos, minX, maxX);

        // Map X position to 0–1
        float t = Mathf.InverseLerp(minX, maxX, clampedX);

        // Map t to volume range
        float newVolume = Mathf.Lerp(minVolume, maxVolume, t);

        // Apply to the bass AudioSource
        audioSourceBass.volume = newVolume;
    }

    private void UpdateDrumTrack()
    {
        // Only play drums when walking or running
        if (walkState == WalkState.Walking || walkState == WalkState.Running)
        {
            // Smoothly fade in if not already audible
            audioSourceDrums.volume = Mathf.Lerp(audioSourceDrums.volume, 1f, Time.deltaTime * 5f);

            if (!audioSourceDrums.isPlaying)
            {
                audioSourceDrums.Play();
            }
        }
        else
        {
            // Smoothly fade out when not walking/running
            audioSourceDrums.volume = Mathf.Lerp(audioSourceDrums.volume, 0f, Time.deltaTime * 5f);

            // Optionally stop when fully silent (to save CPU)
            if (audioSourceDrums.volume < 0.01f && audioSourceDrums.isPlaying)
            {
                audioSourceDrums.Stop();
            }
        }
    }


    /////////////////////Collsion with enemies and check point //////////////////////////////////

    //Get functions
    public bool GetIsHoldingObject()
    {
        return isHoldingObject;
    }

    public bool GetIsGrounded()
    {
        return IsGrounded();
    }

    public float GetLinearVelocityY()
    {
        return rigidBody.linearVelocityY;
    }
}

