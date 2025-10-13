using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    float moveSpeed = 20;
 

    Vector3 zOffset = new Vector3(0, 0, -10);
    Vector3 lookAheadOffset = new Vector3(3, 0, 0);
    int direction = 1;
    float lookAheadOffsetX;
    Vector3 yOffset;
    Vector3 targetX;

    [SerializeField] float zoom = 7;

    float screenUpperLimit;
    float screenLowerLimit;

    void Start()
    {
        this.GetComponent<Camera>().orthographicSize = zoom;
        yOffset = new Vector3 (0, (float)(zoom/3), 0);  //player will always be in the bottom quarter of the screen

        lookAheadOffsetX = (float)(zoom / 3) + zoom; //player will always have about 90% of the screen in front of them

        screenUpperLimit = zoom + zoom / 3 + yOffset.y;
        screenLowerLimit = -zoom + zoom/3 + yOffset.y;
    }

    // Update is called once per frame
    void Update()
    {
        targetX = new Vector3 (target.position.x, 0, 0);
        updateDirection();
        updateLookAheadOffset();
        updateYOffset();

        transform.position = Vector3.MoveTowards(this.transform.position, targetX + lookAheadOffset + yOffset + zOffset, moveSpeed * Time.deltaTime);
    }

    private void updateDirection()
    {
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
        lookAheadOffset = new Vector3(lookAheadOffsetX * direction, 0, 0);
    }

    private void updateYOffset()
    {
        if(target.position.y > screenUpperLimit)
        {
            yOffset.y = yOffset.y + (float)(zoom*1.5);
            screenUpperLimit = screenUpperLimit + (float)(zoom + zoom / 3);
            screenLowerLimit = screenLowerLimit + (float)(zoom + zoom / 3);
        }
        if (target.position.y < screenLowerLimit)
        {
            yOffset.y = yOffset.y - (float)(zoom * 1.5);
            screenUpperLimit = screenUpperLimit - (float)(zoom + zoom / 3);
            screenLowerLimit = screenLowerLimit - (float)(zoom + zoom / 3);
        }
    }





}




//using these tutorials for help
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.MoveTowards.html