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
    }


    [SerializeField] private float moveSpeed = 6.5F;

    [SerializeField] private float jumpHeight = 20F;
    [SerializeField] private float jumpUpwardsGravity = 5f;
    [SerializeField] private float fallingGravity  = 7f;
    bool isJumping = false;
    [SerializeField] float coyoteTime = 0.175f;
    float coyoteTimeCounter;
    float jumpBufferTime = 0.1f;
    float jumpBufferTimer = 0;


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

    public LayerMask groundLayer;

    [SerializeField] public cameraMovement camera;
 


    private bool isRolling = false;
    WalkState walkState = WalkState.Idle;
    Vector3 directionFacing = new Vector3(1, 0, 0);

    //idle = 0 walking between -1 and 1 running = 1

    private Vector3 lastCheckpointPosition;


    private void Start()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
        collidor = this.GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Vector3 lastCheckpointPosition = new Vector3(1f, 1f, 1f); // Default position
        coyoteTimeCounter = coyoteTime;


        StartTracks();

    }

    private void Update()
    {
      
        coyoteTimer();
        updateJumpBufferTimer();

    }

    private void FixedUpdate()
    {
        Move();
        if(isJumping == true)
        {
            JumpPhysics();
        }

    }

    public void onMoveInput(float horizontal)
    {
        this.horizontal= horizontal;
        //Debug.Log(horizontal);

        if (horizontal < 0.8 && horizontal > -0.8 && horizontal!= 0)
        {

            //If the stick is not pushed all the way in either direction
            //(aka the player is not going full speed)
            //and sets the walkState to Walking accordingly
            walkState = WalkState.Walking;
            animator.SetInteger("state", 1);
            //PlayWalkSound();
           if (horizontal < 0)
            {
                directionFacing = new Vector3(-1, 0, 0);
                animator.transform.localScale = new Vector3(-0.3929782f, 0.3929782f, 0.3929782f);
            }else{
                animator.transform.localScale = new Vector3(0.3929782f, 0.3929782f, 0.3929782f);
                directionFacing = new Vector3(1, 0, 0);
            }


        } else if(horizontal == 1 || horizontal == -1) //help I don't know the absolute function in C#
        {
            //if the stick is pushed fully in either direction 
            //(aka the player is going full speed)
            //then walkState is set to Running
            walkState = WalkState.Running;
            animator.SetInteger("state", 2);

            //PlayRunSound();

            if (horizontal < 0)
            {
                directionFacing = new Vector3(-1, 0, 0);
                animator.transform.localScale = new Vector3(-0.3929782f, 0.3929782f, 0.3929782f); //what in the world do these numbers mean
            } else {
                directionFacing = new Vector3(1, 0, 0);
                animator.transform.localScale = new Vector3(0.3929782f, 0.3929782f, 0.3929782f); //maybe make a constant or something
            }
        }
        else if(horizontal == 0)
        {
            //if the stick is not pushed at all
            //(aka the player is not moving)
            //then walkState is set to Idle
            walkState = WalkState.Idle;
            animator.SetInteger("state", 0);
            //StopSound();
        }
        else
        {
            //if we somehow get a different input, they're probably moving so let's go with Running
            walkState = WalkState.Running;
            animator.SetInteger("state", 2);
            // PlayRunSound();

        }

       // Debug.Log(walkState);
       // Debug.Log(horizontal);
    }

    public void onJumpInput()
    {

        //checks if they're still within the grace period of jumping
        if (coyoteTimeCounter > 0)
        {
            isJumping = true;
            rigidBody.linearVelocityY = jumpHeight;
            coyoteTimeCounter = -1;
            animator.SetInteger("state", 3);
            PlayJumpSound();

        }
        else //otherwise, store the jump input for a moment to see if they hit ground
        {
            jumpBuffer();              
        }
            coyoteTimeCounter = -1;
    }

    public void onJumpCanceled()
    {
    
     //cuts the vertical velocity when they let go of the jump button to shorten the jump

     rigidBody.linearVelocityY = rigidBody.linearVelocityY * 0.3f;
     isJumping = false;
       
    }

    private void JumpPhysics()
    {
        //if the player is moving upwards
      if(rigidBody.linearVelocityY > 0)
        {
            rigidBody.gravityScale = jumpUpwardsGravity;
        }
        else
        {
            rigidBody.gravityScale = fallingGravity;
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

    private void jumpBuffer()
    {
        jumpBufferTimer = jumpBufferTime;
    }
    //when jump input is given while in the air, start a short timer 
    //if the player lands on the ground in that time, input the jump
    private void updateJumpBufferTimer()
    {
        jumpBufferTimer-= Time.deltaTime;

        if(jumpBufferTimer > 0 && IsGrounded() == true)
        {
            onJumpInput();
            jumpBufferTimer = -1;
        }
    }
    public void onRollInput()
    {
        Debug.Log("Roll Pressed");
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

    }


    private bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 size = collidor.bounds.size * 0.7f;
        float angle = 0;
        Vector2 direction = Vector2.down;
        float distance = 0.5f;



        //oxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.AllLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity); 
        RaycastHit2D groundCheck = Physics2D.BoxCast(position, size, angle, direction, distance, groundLayer);


        if (groundCheck)
        {
            return true;
        }
            return false;
    }


    private void Move()
    {   
        //avoiding the weird movement when the player is rolling and moving
        if (isRolling) return;

        Vector3 moveDirection = Vector3.right * horizontal;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private System.Collections.IEnumerator Roll()
    {
        Debug.Log("Rolling!");
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


    ///////////////////// Play Music ///////////////////////////////////////////////////////
    public void StartTracks(){
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
        audioSourceDrums.volume = 0f; // start silent
        audioSourceDrums.loop = true;
        audioSourceDrums.Play();
    }

    public void drumVol(){
        audioSourceDrums.volume = Mathf.Abs(horizontal);
    }

    public void PlayJumpSound(){
        //Debug.Log("Enters walksounds function");
        audioSource.PlayOneShot(jumpAudio, 1.0f);

    }
        
    public void PlayRollSound(){
        Debug.Log("Enters rollsounds function");
        audioSource.PlayOneShot(rollAudio, 1.0f);

    }

    /////////////////////Collsion with enemies and check point //////////////////////////////////

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
        {
            transform.position = lastCheckpointPosition;
            camera.SnapToTarget();
        }
    }

}
