using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum WalkState
    {
        Idle,
        Walking,
        Running,
    }

    [SerializeField] private float moveSpeed = 6.5F;
    [SerializeField] private float jumpHeight = 20F;

    float horizontal;

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private BoxCollider2D groundCheckCol;
    [SerializeField] private Animator animator;

    public AudioSource audioSource;
    public AudioClip walkAudio;
    public AudioClip runAudio;
    public AudioClip jumpAudio;
    public AudioClip rollAudio;
    public float volume = 1.0f;

    private bool isGrounded = true;
    WalkState walkState = WalkState.Idle;

    //idle = 0 walking between -1 and 1 running = 1

    private void Start()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Move();
    }
    public void onMoveInput(float horizontal)
    {
        this.horizontal= horizontal;
        Debug.Log(horizontal);

        if (horizontal < 1 && horizontal > -1 && horizontal!= 0)
        {

            //If the stick is not pushed all the way in either direction
            //(aka the player is not going full speed)
            //and sets the walkState to Walking accordingly
            walkState = WalkState.Walking;
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            PlayWalkSound();
           if (horizontal < 0)
            {
                animator.transform.localScale = new Vector3(-0.3929782f, 0.3929782f, 0.3929782f);
            }else{
                animator.transform.localScale = new Vector3(0.3929782f, 0.3929782f, 0.3929782f);
            }


        } else if(horizontal == 1 || horizontal == -1) //help I don't know the absolute function in C#
        {
            //if the stick is pushed fully in either direction 
            //(aka the player is going full speed)
            //then walkState is set to Running
            walkState = WalkState.Running;
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);

            PlayRunSound();

            if (horizontal < 0)
            {
                animator.transform.localScale = new Vector3(-0.3929782f, 0.3929782f, 0.3929782f);
            } else {
                animator.transform.localScale = new Vector3(0.3929782f, 0.3929782f, 0.3929782f);
            }
        }
        else if(horizontal == 0)
        {
            //if the stick is not pushed at all
            //(aka the player is not moving)
            //then walkState is set to Idle
            walkState = WalkState.Idle;
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            StopSound();
        }
        else
        {
            //if we somehow get a different input, they're probably moving so let's go with Running
            walkState = WalkState.Running;
            animator.SetBool("isRunning", true);
            PlayRunSound();

        }

        Debug.Log(walkState);
    }

    public void onJumpInput()
    {
        if(isGrounded == true)
        {
            rigidBody.linearVelocityY = jumpHeight;
            animator.SetBool("isJumping", true);
            PlayJumpSound();
        }

    }

    public void onRollInput()
    {
        Debug.Log("Roll Pressed");
        //Roll code goes in here
        //Remember to connect the player and their functions to
        //the input controller script on the game manager
        if(Input.GetKeyDown(KeyCode.J)) //Input.GetKeyDown(KeyCode.JoystickButton1)
        {
            moveSpeed = 10F;
            animator.SetBool("isRolling", true);
            new WaitForSeconds(1f);
            moveSpeed = 6.5F;
            animator.SetBool("isRolling", false);
        }
        else
        {
            moveSpeed = 6.5F;
            animator.SetBool("isRolling", false);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when the player's groundCheck trigger comes into contact with ground, set isGrounded to true
        if (collision.CompareTag("Ground") == true)
        {
            isGrounded= true;
            animator.SetBool("isJumping", false);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //When the player's groundCheck trigger leaves the ground, change isGrounded to false
        if (collision.CompareTag("Ground") == true)
        {
            isGrounded = false;
            animator.SetBool("isJumping", false);
        }
    }

    private void Move()
    {
        Vector3 moveDirection = Vector3.right * horizontal;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void PlayWalkSound(){
        Debug.Log("Enters walksounds function");

         if (!audioSource.isPlaying){
            audioSource.clip = walkAudio;
            audioSource.volume = 1.0f;
            audioSource.loop = true;      // enable looping
            audioSource.Play();
        }   
    }

    public void PlayRunSound(){
        Debug.Log("Enters runsounds function");

         if (!audioSource.isPlaying){
            audioSource.clip = runAudio;
            audioSource.volume = 1.0f;
            audioSource.loop = true;      // enable looping
            audioSource.Play();
        }   
    }

     public void StopSound(){
        Debug.Log("Enters stop function");

        if (audioSource.isPlaying){
                audioSource.Stop();
                Debug.Log("Stop the music");
        }
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
