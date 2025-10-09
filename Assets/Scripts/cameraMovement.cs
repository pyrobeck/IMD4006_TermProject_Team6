using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveSpeed = 20;
 

    Vector3 zOffset = new Vector3(0, 0, -10);
    Vector3 lookAheadOffset = new Vector3(3, 0, 0);
    int direction = 1;
    [SerializeField] float lookAheadOffsetX = 4;
    Vector3 yOffset;

    [SerializeField] float zoom = 7;

    void Start()
    {
        this.GetComponent<Camera>().orthographicSize = zoom;
        yOffset = new Vector3 (0, zoom/2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        updateDirection();
        updateLookAheadOffset();

        transform.position = Vector3.MoveTowards(this.transform.position, target.position + lookAheadOffset + yOffset + zOffset, moveSpeed * Time.deltaTime);
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
}




//using these tutorials for help
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.MoveTowards.html