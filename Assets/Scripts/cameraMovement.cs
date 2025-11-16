using UnityEngine;
using UnityEngine.UIElements;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] PlayerController targetScript;
    float moveSpeedX = 20;
    float moveSpeedY = 15;
    float standardMoveSpeedX = 20;
    float standardMoveSpeedY = 15;
    float directionFlippingMoveSpeed = 10;


    Vector3 zOffset = new Vector3(0, 0, -10);
    Vector3 lookAheadOffset = new Vector3(3, 0, 0);
    int direction = 1;
    float lookAheadOffsetX;
    Vector3 yOffset;
    Vector3 targetX;
    Vector3 targetY;

    Vector3 cameraTarget;
    float cameraTargetX;
    float cameraTargetY;
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

        yOffset = new Vector3(0, (float)((screenHeight / 5)), 0);  //player will always be in the bottom bit of the screen

        lookAheadOffsetX = (float)screenWidth / 6; //player will always have about 60% of the screen in front of them

        screenUpperLimit = (screenHeight / 2 + yOffset.y);
        screenLowerLimit = (-screenHeight / 2 + yOffset.y) + (screenHeight / 10);

        deadzoneRatio = screenWidth / 12;
        deadzoneRight = deadzoneRatio;
        deadzoneLeft = -deadzoneRatio;
    }

    // Update is called once per frame
    void Update()
    {
        //uncomment these if you need to see the screen limits for level design
        //Debug.DrawRay(new Vector3(0, screenUpperLimit, 0), Vector3.right * 1000);
        //Debug.DrawRay(new Vector3(0, screenLowerLimit, 0), Vector3.right * 1000);
        //and these for the camera movement deadzone
        Debug.DrawRay(new Vector3(deadzoneRight, 0, 0), Vector3.up * 500);
        Debug.DrawRay(new Vector3(deadzoneLeft, 0, 0), Vector3.up * 500);

        updateDirection();
        updateLookAheadOffset();
        updateYOffset();

        updateCameraPosition();

        updateDeadzonePosition();
    }

    private void updateCameraPosition()
    {
        UpdateXCameraPosition();
        UpdateYCameraPosition();

        cameraTargetX = Mathf.MoveTowards(this.transform.position.x, targetX.x, moveSpeedX * Time.deltaTime);
        cameraTargetY = Mathf.MoveTowards(this.transform.position.y, targetY.y, moveSpeedY * Time.deltaTime);

        cameraTarget = new Vector3(cameraTargetX, cameraTargetY, zOffset.z);

        transform.position = cameraTarget;
    }

    private void UpdateXCameraPosition()
    {
        //only update the camera target if the player is outside the camera deadzone
        //this is so the camera smoothly stops following the player's slight horizontal adjustments, but still follows them jumping or falling
        if (!(target.position.x < deadzoneRight && target.position.x > deadzoneLeft))
        {
            targetX = new Vector3(target.position.x, 0, 0) + lookAheadOffset;
            cameraTargetXRecord = targetX; //keep a record of the last horizontal target while the player was outside the deadzone
        }
        else
        {
            targetX = cameraTargetXRecord;
        }
    }
    private void UpdateYCameraPosition()
    {
        targetY = yOffset;
    }
    private void updateDeadzonePosition()
    {
        //if the player is within the camera deadzone, don't move it
        if (target.position.x < deadzoneRight && target.position.x > deadzoneLeft)
        {
            return;
        }
        deadzoneRight = Mathf.Lerp(deadzoneRight, target.position.x + deadzoneRatio, (moveSpeedX * 0.1f) * Time.deltaTime);
        deadzoneLeft = Mathf.Lerp(deadzoneLeft, target.position.x - deadzoneRatio, (moveSpeedX * 0.1f) * Time.deltaTime);
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
                yOffset.y = yOffset.y - (float)(screenHeight * 0.9);
                screenUpperLimit = screenUpperLimit - (float)(screenHeight * 0.9);
                screenLowerLimit = screenLowerLimit - (float)(screenHeight * 0.9);
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
                yOffset.y = yOffset.y + (float)(screenHeight * 0.9);
                screenUpperLimit = screenUpperLimit + (float)(screenHeight * 0.9);
                screenLowerLimit = screenLowerLimit + (float)(screenHeight * 0.9);
            }

        }
    }

    private void checkPlayerDistance()
    {
        if (Mathf.Abs(target.position.x - transform.position.x) <= screenWidth * 1.5)
        {
            return;
        }
        if (Mathf.Abs(target.position.y - transform.position.y) <= screenHeight * 1.5)
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