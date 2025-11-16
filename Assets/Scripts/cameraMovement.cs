using UnityEngine;
using UnityEngine.UIElements;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] PlayerController targetScript;
    float moveSpeed = 20;
    float standardMoveSpeed = 20;
    float directionFlippingMoveSpeed = 10;


    Vector3 zOffset = new Vector3(0, 0, -10);
    Vector3 lookAheadOffset = new Vector3(3, 0, 0);
    int direction = 1;
    float lookAheadOffsetX;
    Vector3 yOffset;
    Vector3 targetX;

    Vector3 cameraTarget;
    Vector3 cameraTargetXRecord;


    [SerializeField] float zoom = 7;
    float screenWidth;
    float screenHeight;

    float screenUpperLimit;
    float screenLowerLimit;

    float deadzoneRight;
    float deadzoneLeft;

    float deadzoneRatio;


    void Start()
    {
        this.GetComponent<Camera>().orthographicSize = zoom;
        screenWidth = zoom * 3.6f;
        screenHeight = zoom * 2;

        yOffset = new Vector3(0, (float)(zoom * 0.5f), 0);  //player will always be in the bottom quarter or so of the screen

        lookAheadOffsetX = (float)zoom / 3; //player will always have about 80% of the screen in front of them

        screenUpperLimit = zoom + yOffset.y;
        screenLowerLimit = -zoom + zoom / 5 + yOffset.y;

        deadzoneRatio = zoom / 5;
        deadzoneRight = deadzoneRatio;
        deadzoneLeft = -deadzoneRatio;
    }

    // Update is called once per frame
    void Update()
    {
        //uncomment these if you need to see the screen limits for level design
        //Debug.DrawRay(new Vector3(0,screenUpperLimit,0), Vector3.right * 1000);
        //Debug.DrawRay(new Vector3(0, screenLowerLimit, 0), Vector3.right * 1000);
        //and these for the camera movement deadzone
        //Debug.DrawRay(new Vector3(deadzoneRight, 0, 0), Vector3.up * 500);
        //Debug.DrawRay(new Vector3(deadzoneLeft, 0, 0), Vector3.up * 500);
        Debug.DrawRay(new Vector3(0, -zoom / 2, 0), Vector3.up * zoom * 2);

        targetX = new Vector3(target.position.x, 0, 0);
        updateDirection();
        updateLookAheadOffset();
        updateYOffset();

        updateCameraPosition();

        updateDeadzonePosition();
    }

    private void updateCameraPosition()
    {
        //only update the camera target if the player is outside the camera deadzone
        //this is so the camera smoothly stops following the player's slight horizontal adjustments, but still follows them jumping or falling
        if (!(target.position.x < deadzoneRight && target.position.x > deadzoneLeft))
        {
            cameraTarget = targetX + lookAheadOffset + yOffset + zOffset;
            cameraTargetXRecord = new Vector3(cameraTarget.x, 0, 0); //keep a record of the last horizontal target while the player was outside the deadzone
        }
        else
        {
            cameraTarget = cameraTargetXRecord + yOffset + zOffset;
        }

        transform.position = Vector3.MoveTowards(this.transform.position, cameraTarget, moveSpeed * Time.deltaTime);
    }

    private void updateDeadzonePosition()
    {
        //if the player is within the camera deadzone, don't move it
        if (target.position.x < deadzoneRight && target.position.x > deadzoneLeft)
        {
            return;
        }
        deadzoneRight = Mathf.Lerp(deadzoneRight, target.position.x + deadzoneRatio, (moveSpeed * 0.1f) * Time.deltaTime);
        deadzoneLeft = Mathf.Lerp(deadzoneLeft, target.position.x - deadzoneRatio, (moveSpeed * 0.1f) * Time.deltaTime);
    }
    private void updateDirection()
    {
        //if the player is within the camera deadzone, don't update direction
        if (target.position.x < deadzoneRight && target.position.x > deadzoneLeft)
        {
            // return;
        }

        if (target.localScale.x < 0)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
    }

    private void updateLookAheadOffset()
    {
        //slowly updates the lookahead to the direction the player is headed
        lookAheadOffset = Vector3.MoveTowards(lookAheadOffset, new Vector3(lookAheadOffsetX * direction, 0, 0), directionFlippingMoveSpeed * Time.deltaTime);
    }

    private void updateYOffset()
    {
        //don't update camera vertically unless player is either grounded or falling
        if (targetScript.GetIsGrounded() == false && targetScript.GetLinearVelocityY() >= 0)
        {
            return;
        }

        if (target.position.y < screenLowerLimit)
        {
            while (target.position.y < screenLowerLimit)
            {
                yOffset.y = yOffset.y - (float)(zoom * 1.5);
                screenUpperLimit = screenUpperLimit - (float)(zoom * 1.5);
                screenLowerLimit = screenLowerLimit - (float)(zoom * 1.5);
            }

        }
        // if the player is falling, then don't move the camera upwards
        if (targetScript.GetLinearVelocityY() < 0)
        {
            return;
        }
        if (target.position.y > screenUpperLimit)
        {
            while (target.position.y > screenUpperLimit)
            {
                yOffset.y = yOffset.y + (float)(zoom * 1.5);
                screenUpperLimit = screenUpperLimit + (float)(zoom * 1.5);
                screenLowerLimit = screenLowerLimit + (float)(zoom * 1.5);
            }

        }
    }

    private void checkPlayerDistance()
    {
        if (Mathf.Abs(target.position.x - transform.position.x) <= zoom * 2)
        {
            return;
        }
        if (Mathf.Abs(target.position.y - transform.position.y) <= zoom * 2)
        {
            return;
        }
    }

    public void SnapToTarget()
    {
        updateDirection();
        updateLookAheadOffset();
        updateYOffset();

        Vector3 snapPos = new Vector3(target.position.x, target.position.y, 0) + lookAheadOffset + yOffset + zOffset;

        transform.position = snapPos;
    }

}




//using these tutorials for help
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.MoveTowards.html