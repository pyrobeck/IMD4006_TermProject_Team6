using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
        Dance1, //6
        Dance2, //7
        Dance3, //8
        Dance4, //9
        Idle1, //10
        Idle2, //11
        Moonwalk //12
    }


    [SerializeField] private float moveSpeed = 6.5F;
    [SerializeField] private float rollSpeed = 20F;
    [SerializeField] private float jumpHeight = 20F;
    [SerializeField] private float wallJumpDistance = 5f;
    [SerializeField] private float jumpUpwardsGravity = 5f;
    [SerializeField] private float fallingGravity = 7f;
    [SerializeField] private float wallSlidingGravity = 1.5f;
    private bool isJumping = false;
    private bool hasLeftGround = false;
    private bool isWallJumping = false;
    private bool isRolling = false;
    private bool isDancing = false;
    private float currentRollSpeed = 0;
    private float lastRollSpeed = 0;
    [SerializeField] float coyoteTime = 0.175f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferTimer = 0;
    private float wallJumpTimer = 0;
    private float maxWallJumpTime = 0.5f;
    private float rollDuration = 0.6F;
    private float rollTimer = 0;
    private float idleAnimationTimer;
    private float timeUntilIdleAnimation = 5f;





    float horizontal;

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private CapsuleCollider2D collidor;
    [SerializeField] private Animator animator;

    [Header("Player Sound Effects")]
    public AudioSource sfxSource;
    public AudioClip jumpAudio;
    public AudioClip rollAudio;
    public AudioClip deathAudio;
    public float volume = 0.5f;

    public GameObject deathImage;


    public LayerMask groundLayer;

    [SerializeField] public cameraMovement camera;

    [SerializeField] private GameObject heldObjectPrefab;
    [SerializeField] private GameObject thrownObjectPrefab;
    private bool isHoldingObject = false;
    private bool isNearJunkPile = false;
    private bool isThrowingObject = false;
    private float throwCooldownTimer = 0;
    private float maxThrowCooldownTimer = 0.5f;


    WalkState walkState = WalkState.Idle;
    Vector3 directionFacing = new Vector3(1, 0, 0);
    Vector3 spriteScale = new Vector3(0.3929782f, 0.3929782f, 0.3929782f);
    Vector3 spriteScaleFlipped = new Vector3(-0.3929782f, 0.3929782f, 0.3929782f);


    //idle = 0 walking between -1 and 1 running = 1

    private Vector3 lastCheckpointPosition;


    SpriteRenderer temp;


    private WalkState lastState;

    private void Start()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
        collidor = this.GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        sfxSource = GetComponent<AudioSource>();
        Vector3 lastCheckpointPosition = new Vector3(1f, 1f, 1f); // Default position
        coyoteTimeCounter = coyoteTime;
        idleAnimationTimer = timeUntilIdleAnimation;
        spriteScale = this.transform.localScale;
        spriteScaleFlipped = new Vector3(-spriteScale.x, spriteScale.y, spriteScale.z);

        temp = this.GetComponent<SpriteRenderer>();
        lastState = walkState;

        deathImage.SetActive(false);

    }

    private void Update()
    {
        UpdateTimers();
        TrackCurrentRollSpeed();
        SetWalkStateAfterJumping();
        IdleAnimations();
        UpdateAnimationState();
    }


    private void FixedUpdate()
    {

        if (IsGrounded() == false)
        {
            JumpMidairPhysics();
        }
        Move();
    }



    public void onMoveInput(float horizontal)
    {
        this.horizontal = horizontal;

        SetDirection();
        SetWalkState();
        if (isDancing)
        {
            walkState = WalkState.Moonwalk;
        }
    }

    public void OnDanceInput(Vector2 stickInput)
    {

        isDancing = true;

        if (walkState == WalkState.Running || walkState == WalkState.Walking || walkState == WalkState.Moonwalk)
        {
            walkState = WalkState.Moonwalk;
            return;
        }

        if (stickInput.x >= -0.5 && stickInput.x <= 0.5 && stickInput.y <= -0.5) //down
        {
            walkState = WalkState.Dance1;
        }
        if (stickInput.x <= -0.5 && stickInput.y >= -0.5 && stickInput.y <= 0.5)//left
        {
            walkState = WalkState.Dance2;
        }
        if (stickInput.x >= -0.5 && stickInput.x <= 0.5 && stickInput.y >= 0.5) //up
        {
            walkState = WalkState.Dance3;
        }
        if (stickInput.x >= 0.5 && stickInput.y <= 0.5 && stickInput.y >= -0.5) //right
        {
            walkState = WalkState.Dance4;
        }
    }

    public void OnDanceCancelled()
    {
        isDancing = false;
        if (walkState == WalkState.Moonwalk)
        {
            SetWalkState();
        }
        else
        {
            walkState = WalkState.Idle;
        }
    }
    public void onJumpInput()
    {
        if (isDancing) return;
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

    public void onRollInput()
    {
        if (isRolling == true)
        {
            return;
        }
        walkState = WalkState.Rolling;
        Roll();
    }

    private void Roll()
    {
        isRolling = true;
        rigidBody.linearVelocityX = rollSpeed * directionFacing.x;
    }

    private void TrackCurrentRollSpeed()
    {
        if (isRolling == true)
        {
            lastRollSpeed = currentRollSpeed;
            currentRollSpeed = rigidBody.linearVelocityX;
        }
    }
    private void Jump()
    {
        if (isRolling == true)
        {
            isRolling = false;
        }
        isJumping = true;
        rigidBody.linearVelocityY = jumpHeight;
        coyoteTimeCounter = -1;
        walkState = WalkState.Jumping;
        PlayJumpSound();
    }

    private void WallJump()
    {
        if (isRolling) //idk how they'd do this one but I think it should cancel the roll anyway
        {
            isRolling = false;
        }
        //adds a force away from the wall they're jumping from
        rigidBody.linearVelocityX = wallJumpDistance * -directionFacing.x;
        isWallJumping = true;
        ResetWallJumpTimer();
        ReverseDirection();
        Jump();
    }

    private void JumpMidairPhysics()
    {
        if (rigidBody.linearVelocityY < -20) //stop the player from falling too fast
        {
            rigidBody.linearVelocityY = -20;
        }
        //if the player is moving upwards
        if (rigidBody.linearVelocityY > 0)
        {
            rigidBody.gravityScale = jumpUpwardsGravity;
            return;
        }

        if (IsWallSliding() == true)
        {
            if (rigidBody.linearVelocityY < -5)
            {
                rigidBody.linearVelocityY = -5;
            }
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


    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Store the position of the checkpoint
        if (collision.CompareTag("Respawn"))
        {
            lastCheckpointPosition = new Vector3(collision.transform.position.x, collision.transform.position.y, transform.position.z);
            //Debug.Log("Checkpoint reached at: " + lastCheckpointPosition);

            foreach (Checkpoint cp in FindObjectsOfType<Checkpoint>())
                cp.SetInactive();

            Checkpoint thisCheckpoint = collision.GetComponent<Checkpoint>();
            if (thisCheckpoint != null)
                thisCheckpoint.SetActive();
        }

        if (collision.CompareTag("Enemies"))
        {
            if (walkState == WalkState.Rolling)
            {
                dieToRoll enemy = collision.gameObject.GetComponent<dieToRoll>();
                if (enemy != null)
                {
                    enemy.Die();
                    return;
                }
            }
            StartCoroutine(playerDeath());

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
            if (walkState == WalkState.Rolling)
            {
                dieToRoll enemy = collision.gameObject.GetComponent<dieToRoll>();
                if (enemy != null)
                {
                    enemy.Die();
                    rigidBody.linearVelocityX = lastRollSpeed;
                    return;
                }
            }
            StartCoroutine(playerDeath());
        }

    }

    private bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 size = new Vector2(collidor.bounds.size.x * 0.1f, collidor.bounds.size.y);
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
            return true;
        }
        return false;
    }

    private bool IsWallSliding()
    {
        if (IsNextToWall() && IsGrounded() == false)
        {
            walkState = WalkState.WallSliding;
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
        if (IsGrounded() == false)
        {
            return;
        }
        if (horizontal == 0)
        {
            walkState = WalkState.Idle;
            return;
        }
        if (horizontal < 0.8 && horizontal > -0.8)
        {
            walkState = WalkState.Walking;
            return;
        }
        if (horizontal >= 1 || horizontal <= -1) //help I don't know the absolute function in C#
        {
            walkState = WalkState.Running;
            return;
        }
    }

    private void SetWalkStateAfterJumping()
    {
        if (walkState != WalkState.Jumping)
        {
            return;
        }

        if (hasLeftGround == false)
        {
            if (IsGrounded() == false)
            {
                hasLeftGround = true;
            }
        }

        if (hasLeftGround == true)
        {
            if (IsGrounded() == true)
            {
                SetWalkState();
                hasLeftGround = false;
            }
        }
    }

    private void IdleAnimations()
    {
        if (horizontal == 0 && rigidBody.linearVelocity == Vector2.zero && IsGrounded() && isDancing == false && isJumping == false)
        {
            walkState = WalkState.Idle;
        }

        if (idleAnimationTimer <= 0)
        {
            int randomIdleAnimation = Random.Range(0, 2);
            if (randomIdleAnimation == 1) // originally picked between the 2 or none but idle2 is haunted......
            {
                walkState = WalkState.Idle1;
            }
            else
            {
                walkState = WalkState.Idle;
                ResetIdleAnimationTimer();
            }



        }


    }
    private void UpdateAnimationState()
    {
        if (lastState != walkState)
        {
            switch (walkState)
            {
                case WalkState.Idle:
                    animator.SetInteger("state", 0);
                    break;
                case WalkState.Walking:
                    animator.SetInteger("state", 1);
                    break;
                case WalkState.Running:
                    animator.SetInteger("state", 2);
                    break;
                case WalkState.Jumping:
                    animator.SetInteger("state", 3);
                    break;
                case WalkState.Rolling:
                    animator.SetInteger("state", 4);
                    break;
                case WalkState.WallSliding:
                    animator.SetInteger("state", 0);
                    break;
                case WalkState.Dance1:
                    animator.SetInteger("state", 6);
                    break;
                case WalkState.Dance2:
                    animator.SetInteger("state", 7);
                    break;
                case WalkState.Dance3:
                    animator.SetInteger("state", 8);
                    break;
                case WalkState.Dance4:
                    animator.SetInteger("state", 9);
                    break;
                case WalkState.Idle1:
                    animator.SetInteger("state", 10);
                    break;
                case WalkState.Idle2:
                    animator.SetInteger("state", 11);
                    break;
                case WalkState.Moonwalk:
                    animator.SetInteger("state", 12);
                    break;
                default:
                    animator.SetInteger("state", 0);
                    break;
            }
            lastState = walkState;
        }
    }

    /// Timers
    private void UpdateTimers()
    {
        coyoteTimer();
        updateJumpBufferTimer();
        WallJumpTimer();
        ThrowCooldownTimer();
        RollTimer();
        IdleAnimationTimer();
    }

    private void ResetIdleAnimationTimer()
    {
        idleAnimationTimer = timeUntilIdleAnimation;
    }
    private void IdleAnimationTimer()
    {
        if (walkState == WalkState.Idle)
        {
            idleAnimationTimer -= Time.deltaTime;
        }
        else
        {
            idleAnimationTimer = timeUntilIdleAnimation;
        }
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

    private void RollTimer()
    {
        if (isRolling == true)
        {
            rollTimer -= Time.deltaTime;
        }
        else if (rollTimer != rollDuration)
        {
            rollTimer = rollDuration;
        }
        if (rollTimer <= 0)
        {
            isRolling = false;
            SetWalkState();
        }
    }
    private void ResetWallJumpTimer()
    {
        wallJumpTimer = maxWallJumpTime;
    }

    public void PlayJumpSound()
    {
        sfxSource.PlayOneShot(jumpAudio, 0.8f);
    }

    public void PlayRollSound()
    {
        sfxSource.PlayOneShot(rollAudio, 0.8f);
    }


    //Get functions
    public bool GetIsHoldingObject()
    {
        return isHoldingObject;
    }

    public bool GetIsGrounded()
    {
        return IsGrounded();
    }

    public bool GetIsWallJumping()
    {
        return isWallJumping;
    }

    public float GetLinearVelocityY()
    {
        return rigidBody.linearVelocityY;
    }

    public void RemoveHeldObjects()
    {
        isHoldingObject = false;
        isThrowingObject = false;
        ClearHeldObjectsFromChildren();
    }

    public void KillPlayer()
    {
        StartCoroutine(DeathOnBossLevel());
    }

    private IEnumerator DeathOnBossLevel()
    {
        sfxSource.PlayOneShot(deathAudio, 0.8f);
        yield return new WaitForSeconds(0.1f);
        transform.position = new Vector3(0, -50, 0);
        deathImage.SetActive(true);


        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private IEnumerator playerDeath()
    {
        sfxSource.PlayOneShot(deathAudio, 0.8f);
        yield return new WaitForSeconds(0.1f);
        transform.position = new Vector3(0, -50, 0);
        deathImage.SetActive(true);


        yield return new WaitForSeconds(2f);

        isHoldingObject = false;
        ClearHeldObjectsFromChildren();
        transform.position = lastCheckpointPosition;
        deathImage.SetActive(false);
        camera.SnapToTarget();
        // Debug.Log("Hit enemy! Respawning at: " + lastCheckpointPosition);
    }
}

