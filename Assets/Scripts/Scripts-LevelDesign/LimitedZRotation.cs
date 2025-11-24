using UnityEngine;

public class AutoLimitedZRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float baseRotation = -180f;
    public float minRotation = -230f;
    public float maxRotation = -130f;
    public float rotationSpeed = 1f; // how fast it swings

    private float rotationStart;
    private int direction;
    private float range;

    void Start()
    {
        // Calculate total range
        range = maxRotation - minRotation;

        if (rotationSpeed < 0)
        {
            rotationStart = maxRotation;
            direction = -1;
        }
        else
        {
            rotationStart = minRotation;
            direction = 1;
        }
    }

    void Update()
    {
        // PingPong value oscillates between 0 and range
        float angle = rotationStart + (direction * Mathf.PingPong(Time.time * rotationSpeed, range));

        // Apply to Z rotation
        transform.localEulerAngles = new Vector3(0f, 0f, angle);
    }
}
