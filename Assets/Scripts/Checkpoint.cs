using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isActive = false;

    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private Color activeColor = Color.yellow;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetInactive();
    }

    public void SetActive()
    {
        isActive = true;
        spriteRenderer.color = activeColor;
    }

    public void SetInactive()
    {
        isActive = false;
        spriteRenderer.color = inactiveColor;
    }
}
