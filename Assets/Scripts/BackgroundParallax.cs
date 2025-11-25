using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private float parallaxFactorX = 1;
    [SerializeField] private float parallaxFactorY = 1;

    private SpriteRenderer spriteRenderer;
    Vector2 spriteSize;
    private Vector2 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        spriteSize = spriteRenderer.bounds.size;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 cameraPosition = camera.transform.position;
        float relativePosition = cameraPosition.x * parallaxFactorX;
        float distanceBetweenCameraAndRelativePosition = cameraPosition.x - relativePosition;


        Vector3 NewPosition = new Vector3(startPos.x + relativePosition, transform.position.y, transform.position.z);

        transform.position = NewPosition;




        //I don't know why nothing works
        // if (distanceBetweenCameraAndRelativePosition > spriteSize.x)
        // {
        //     startPos.x += spriteSize.x;
        // }
        // else if (distanceBetweenCameraAndRelativePosition < -spriteSize.x)
        // {
        //     startPos.x -= spriteSize.x;
        // }
    }
    //most tutorials are basically the same but I leaned most heavily on this tutorial
    //https://blog.yarsalabs.com/parallax-effect-in-unity-2d/
}
