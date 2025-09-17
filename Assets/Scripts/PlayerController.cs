using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6.5F;
    [SerializeField] private float jumpHeight = 5F;

    float horizontal;
 

    private void Update()
    {
        Vector3 moveDirection = Vector3.right * horizontal;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    public void onMoveInput(float horizontal)
    {
        this.horizontal= horizontal;
        Debug.Log(horizontal);
    }

    public void onJumpInput()
    {
        //WILL BE UPDATED TO BE AN ACTUAL JUMP
        //This is just a placeholder lol
        transform.position += Vector3.up * jumpHeight;
    }

    public void onRollInput()
    {
        Debug.Log("Roll Pressed");
        //Roll code goes in here
        //Remember to connect the player and their functions to
        //the input controller script on the game manager


    }
}
