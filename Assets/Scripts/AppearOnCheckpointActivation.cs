using UnityEngine;

public class AppearOnCheckpointActivation : MonoBehaviour
{

    [SerializeField] private NewCheckpoint checkpoint;
    private SpriteRenderer sprite;
    private Vector2 spriteSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        if (sprite == null)
        {
            return;
        }
        spriteSize = sprite.size;
        sprite.size = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (sprite == null)
        {
            return;
        }
        if (sprite.size == spriteSize)
        {
            return;
        }

        if (checkpoint.GetIsActive() == true)
        {
            sprite.size = spriteSize;
        }
    }
}
