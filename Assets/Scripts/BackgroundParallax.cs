using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private float parallaxFactorX = 1;
    [SerializeField] private float parallaxFactorY = 1;

    private SpriteRenderer spriteRenderer;
    private float spriteLength;
    private float spriteHeight;
    Vector2 spriteSize;
    private float startPosX;
    private float startPosY;

    private Vector2 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteLength = spriteRenderer.bounds.size.x;
        spriteHeight = spriteRenderer.bounds.size.y;
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        startPos = transform.position;
        spriteSize = spriteRenderer.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cameraPosition = camera.transform.position;

        float distanceX = cameraPosition.x * parallaxFactorX;
        float tempX = cameraPosition.x - distanceX;

        float newBackgroundPositionX = startPos.x + distanceX;

        float distanceY = cameraPosition.y * parallaxFactorY;
        float tempY = cameraPosition.y - distanceY;

        float newBackgroundPositionY = startPos.y + distanceY;


        transform.position = new Vector3(newBackgroundPositionX, newBackgroundPositionY, 100);

        if (tempX > startPosX + spriteSize.x)
        {
            startPosX = startPosX + spriteSize.x;
        }
        else if (tempX < startPosX - spriteSize.x)
        {
            startPosX = startPosX - spriteSize.x;
        }

        if (tempY > startPosY + spriteHeight)
        {
            startPosY = startPosY + spriteHeight;
        }
        else if (tempY < startPosY - spriteHeight)
        {
            startPosY = startPosY - spriteHeight;
        }
    }

}
