using UnityEngine;

public class stickPlayerToPlatform : MonoBehaviour
{
    public Vector3 DeltaPosition { get; private set; }
    private Vector3 oldPosition;
    private Vector3 currentPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oldPosition = transform.position;
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        oldPosition = currentPosition;
        currentPosition = transform.position;
        DeltaPosition = currentPosition - oldPosition;
    }
}
