using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum WalkState
    {
        Idle, //0
        Walking, //1
        Running, //2
        Jumping, //3
        Rolling //4
    }


    [SerializeField] private float moveSpeed = 6.5F;
    [SerializeField] private float jumpHeight = 20F;
    [SerializeField] private float rollSpeed = 10F;
    [SerializeField] private float rollDuration = 0.6F;

    float horizontal;

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private BoxCollider2D groundCheckCol;
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

    private bool isGrounded = true;
    private bool isRolling = false;
    WalkState walkState = WalkState.Idle;
    Vector3 directionFacing = new Vector3(1, 0, 0);

    //idle = 0 walking between -1 and 1 running = 1

    private Vector3 lastCheckpointPosition;


    private void Start()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Vector3 lastCheckpointPosition = new Vector3(1f, 1f, 1f); // Default position

        StartTracks();

    }

    private void Update()
    {
       
        drumVol();

        if (Input.GetKeyDown(KeyCode.JoystickButton1) && !isRolling && isGrounded)
        {
            StartCoroutine(Roll());
        }
    }

    private void FixedUpdate()
    {
        Move();
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
        if(isGrounded == true)
        {
            rigidBody.linearVelocityY = jumpHeight;
            animator.SetInteger("state", 3);
            PlayJumpSound();
        }

    }

    public void onRollInput()
    {
        Debug.Log("Roll Pressed");
        //Roll code goes in here
        //Remember to connect the player and their functions to
        //the input controller script on the game manager
        if (!isRolling && isGrounded)
        {
            PlayRollSound();
            StartCoroutine(Roll());
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when the player's groundCheck trigger comes into contact with ground, set isGrounded to true
        if (collision.CompareTag("Ground") == true)
        {
            isGrounded= true;
            animator.SetInteger("state", 0);
        }
        // Store the position of the checkpoint
        if (collision.CompareTag("Respawn") == true) {
            lastCheckpointPosition = collision.transform.position;
            Debug.Log("Checkpoint reached at: " + lastCheckpointPosition);


            // Deactivate all other checkpoints
            foreach (Checkpoint cp in FindObjectsOfType<Checkpoint>()) { cp.SetInactive(); }
            
            Checkpoint thisCheckpoint = collision.GetComponent<Checkpoint>();
                if (thisCheckpoint != null) {
                    thisCheckpoint.SetActive();
                }
        }

        // If collides with enemy, go back to checkpoint
        if (collision.CompareTag("Enemies") == true) {
            Vector3 respawnPos = lastCheckpointPosition;
            respawnPos.y += 5;
            transform.position = respawnPos;

            Debug.Log("Hit enemy! Respawning at: " + respawnPos);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Camera snap when leaving checkpoint trigger
        if (collision.CompareTag("Respawn") == true) 
        {
            cameraMovement cam = Camera.main.GetComponent<cameraMovement>();
            cam.SnapToTarget();
        }

        // Ground exit detection
        if (collision.CompareTag("Ground") == true)
        {
            isGrounded = false;
            animator.SetInteger("state", 3);
        }
    }


    private void Move()
    {
        Vector3 moveDirection = Vector3.right * horizontal;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private System.Collections.IEnumerator Roll()
    {
        //the actual rolling is done here; aka making it faster for the time used
        Debug.Log("Rolling!");
        isRolling = true;
        animator.SetInteger("state", 4);

        float originalSpeed = moveSpeed;
        moveSpeed = rollSpeed;

        yield return new WaitForSeconds(rollDuration);

        moveSpeed = originalSpeed;
        animator.SetInteger("state", 0);
        isRolling = false;
    }

///////////////////// Play Music ///////////////////////////////////////////////////////
    public void StartTracks(){
             // 🎶 Start background tracks
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
        Debug.Log("Enters walksounds function");
        audioSource.PlayOneShot(jumpAudio, 1.0f);

    }
        
    public void PlayRollSound(){
        Debug.Log("Enters rollsounds function");
        audioSource.PlayOneShot(rollAudio, 1.0f);

    }

}
