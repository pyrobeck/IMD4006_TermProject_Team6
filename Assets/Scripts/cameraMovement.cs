using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] PlayerController targetScript;
    float moveSpeedX = 20;
    [SerializeField] float moveSpeedY = 3;
    float standardMoveSpeedX = 20;
    float standardMoveSpeedY = 3;
    [SerializeField] float fallingMoveSpeedY = 30;
    [SerializeField] float fastMoveSpeedY = 20;
    float directionFlippingMoveSpeed = 10;


    Vector3 zOffset = new Vector3(0, 0, -10);
    Vector3 lookAheadOffset = new Vector3(3, 0, 0);
    int direction = 1;
    float lookAheadOffsetX;
    Vector3 yOffset;
    Vector3 naturalYOffset;
    float cameraBaseline;
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
    float topOfScreen;
    float bottomOfScreen;
    float screenLimitDistance;

    float deadzoneRight;
    float deadzoneLeft;

    float deadzoneRatio;

    bool isFalling = false;
    bool isAboveScren = false;

    void Start()
    {
        this.GetComponent<Camera>().orthographicSize = zoom;
        screenWidth = zoom * 3.6f;
        screenHeight = zoom * 2;

        lookAheadOffsetX = (float)screenWidth / 6; //player will always have about 60% of the screen in front of them


        cameraBaseline = target.position.y;

        screenUpperLimit = cameraBaseline + (screenHeight * (4f / 6f));
        screenLowerLimit = cameraBaseline - (screenHeight * (1f / 6f));
        screenLimitDistance = MathF.Abs(screenLowerLimit - screenUpperLimit);
        yOffset.y = screenLowerLimit + (screenLimitDistance * 0.6f);


        deadzoneRatio = screenWidth / 12;
        deadzoneRight = deadzoneRatio;
        deadzoneLeft = -deadzoneRatio;

        SnapToTarget();
    }

    // Update is called once per frame
    void Update()
    {
        //uncomment these if you need to see the screen limits for level design
        Debug.DrawRay(new Vector3(0, screenUpperLimit, 0), Vector3.right * 1000, Color.red);
        Debug.DrawRay(new Vector3(0, screenLowerLimit, 0), Vector3.right * 1000, Color.blue);
        //Debug.DrawRay(new Vector3(0, cameraBaseline, 0), Vector3.right * 1000, Color.gray);
        Debug.DrawRay(yOffset, Vector3.right * 1000, Color.yellow);
        //and these for the camera movement deadzone
        //Debug.DrawRay(new Vector3(deadzoneRight, 0, 0), Vector3.up * 500);
        //Debug.DrawRay(new Vector3(deadzoneLeft, 0, 0), Vector3.up * 500);

        UpdateDirection();
        UpdateLookAheadOffset();
        UpdateScreenBoundaries();
        UpdateMoveSpeedY();
        updateCameraPosition();

        UpdateDeadzonePosition();
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
    private void UpdateDeadzonePosition()
    {
        //if the player is within the camera deadzone, don't move it
        if (target.position.x < deadzoneRight && target.position.x > deadzoneLeft)
        {
            return;
        }
        deadzoneRight = Mathf.Lerp(deadzoneRight, target.position.x + deadzoneRatio, (moveSpeedX * 0.1f) * Time.deltaTime);
        deadzoneLeft = Mathf.Lerp(deadzoneLeft, target.position.x - deadzoneRatio, (moveSpeedX * 0.1f) * Time.deltaTime);
    }
    private void UpdateDirection()
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

    private void UpdateLookAheadOffset()
    {
        //slowly updates the lookahead to the direction the player is headed
        lookAheadOffset = Vector3.MoveTowards(lookAheadOffset, new Vector3(lookAheadOffsetX * direction, 0, 0), directionFlippingMoveSpeed * Time.deltaTime);
    }

    private void UpdateYOffset()
    {
        yOffset.y = screenLowerLimit + screenLimitDistance * 0.6f;
    }


    private void UpdateScreenBoundaries()
    {
        if (target.position.y < screenUpperLimit && target.position.y > screenLowerLimit)
        {
            screenUpperLimit = cameraBaseline + (screenHeight * (4f / 6f));
            screenLowerLimit = cameraBaseline - (screenHeight * (1f / 6f));
            UpdateYOffset();
        }
        if (target.position.y > screenUpperLimit)
        {
            screenUpperLimit = target.position.y;
            screenLowerLimit = screenUpperLimit - screenLimitDistance;
            UpdateYOffset();
        }

        if (target.position.y < screenLowerLimit)
        {
            screenLowerLimit = target.position.y;
            screenUpperLimit = screenLowerLimit + screenLimitDistance;
            UpdateYOffset();
        }


    }
    private void ResetYOffset()
    {
        yOffset.y = screenLowerLimit + screenLimitDistance * 0.6f;
    }

    private void UpdateMoveSpeedY()
    {
        if (moveSpeedY > standardMoveSpeedY)
        {
            moveSpeedY = moveSpeedY + 2;
            if (target.position.y > transform.position.y - screenHeight * (1f / 6f))
            {
                moveSpeedY = standardMoveSpeedY;
            }
        }
        if (target.position.y < transform.position.y - screenHeight / 2)
        {
            moveSpeedY = fallingMoveSpeedY;
        }

    }


    public void SnapToTarget()
    {
        UpdateDirection();
        UpdateLookAheadOffset();
        ResetYOffset();
        UpdateXCameraPosition();
        UpdateYCameraPosition();

        Vector3 snapPos = new Vector3(targetX.x, targetY.y, zOffset.z);

        transform.position = snapPos;
    }

}




//using these tutorials for help
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.MoveTowards.html