using UnityEngine;

public class RollMovement : MonoBehaviour
{
    public float rollForce = 10f;        
    public AudioSource rollSound;
    public animator rollanimation; //trying to add in the roll animation

    private Rigidbody rb;
    private bool isRolling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(1) || Input.GetButtonDown("Fire3")))
        {
            Roll();
        }
    }

    void Roll()
    {
        isRolling = true;

        Vector3 rollDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (rollDirection == Vector3.zero)
        {
            rollDirection = transform.forward;
        }

        rb.AddForce(rollDirection.normalized * rollForce, ForceMode.VelocityChange);
        
        //playing the audio
        if (rollSound != null) rollSound.Play();
    }

    void EndRoll()
    {
        isRolling = false;
    }
}
