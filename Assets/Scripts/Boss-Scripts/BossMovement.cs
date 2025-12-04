using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Events;

public class BossMovement : MonoBehaviour
{
    [SerializeField] Vector3[] moveLocations;
    public UnityEvent BossMoving;
    private float moveSpeed = 8;
    private SpriteRenderer sprite;
    private Animator animator;
    private int currentLocation = 0;
    private bool isMoving = false;
    private bool isTheFinalHit = false;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }
    public void GetHit()
    {
        StartCoroutine(HitAnimation());
    }

    private IEnumerator HitAnimation()
    {
        animator.SetInteger("state", 1);
        yield return new WaitForSeconds(1.5f);
        animator.SetInteger("state", 0);
        StartCoroutine(GetAngry());
    }

    private IEnumerator GetAngry()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(2f);
        sprite.color = Color.white;
        isMoving = true;
        BossMoving.Invoke();
    }


    private void Move()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveLocations[currentLocation], Time.deltaTime * moveSpeed);

            if (transform.position == moveLocations[currentLocation])
            {
                isMoving = false;
                currentLocation++;
                if (currentLocation == moveLocations.Length)
                {
                    isTheFinalHit = true;
                }
            }
        }
    }
}
