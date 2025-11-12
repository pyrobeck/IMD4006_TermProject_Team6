using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isActive = false;

    [Header("Checkpoint Sprites")]
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetInactive();
    }

    public void SetActive()
    {
        isActive = true;
        spriteRenderer.sprite = activeSprite;
    }

    public void SetInactive()
    {
        isActive = false;
        spriteRenderer.sprite = inactiveSprite;
    }
}
