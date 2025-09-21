using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum WalkState
    {
        Idle,
        Walking,
        Running
    }

    [SerializeField] private float moveSpeed = 6.5F;
    [SerializeField] private float jumpHeight = 20F;

    float horizontal;

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private BoxCollider2D groundCheckCol;
    [SerializeField] private Animator animator;

    

    private bool isGrounded = true;
    WalkState walkState = WalkState.Idle;

    //idle = 0 walking between -1 and 1 running = 1

    private void Start()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

            //going left
            if (horizontal < 0){
                animator.transform.Rotate(0, 180, 0);
            }
            //going right
            if (horizontal > 0) {
                animator.transform.Rotate(0, 180, 0);
            }


        } else if(horizontal == 1 || horizontal == -1) //help I don't know the absolute function in C#
        {
            //if the stick is pushed fully in either direction 
            //(aka the player is going full speed)
            //then walkState is set to Running
            walkState = WalkState.Running;
            animator.SetBool("isRunning", true);

            //going left
            if (horizontal == -1)
            {
                animator.transform.localScale = new Vector3(-0.3929782f, 0.3929782f, 0.3929782f);
            }
            //going right
            if (horizontal == 1)
            {
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
        }
        else
        {
            //if we somehow get a different input, they're probably moving so let's go with Running
            walkState = WalkState.Running;
            animator.SetBool("isRunning", true);
        }

        Debug.Log(walkState);
    }

    public void onJumpInput()
    {
        if(isGrounded == true)
        {
          rigidBody.linearVelocityY = jumpHeight;
          animator.SetBool("isJumping", true);
        }

    }

    public void onRollInput()
    {
        Debug.Log("Roll Pressed");
        //Roll code goes in here
        //Remember to connect the player and their functions to
        //the input controller script on the game manager


        animator.SetBool("isRolling", true);
        animator.SetBool("isRolling", false);

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
}