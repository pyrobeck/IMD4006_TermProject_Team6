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
    private int currentLocation = 1;
    private bool isMoving = false;
    private bool isTheFinalHit = false;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        transform.position = Vector3.up * 99999;
    }

    void Update()
    {
        Move();
    }
    public void GetHit()
    {
        StartCoroutine(HitAnimation());
    }

    public void StartBoss()
    {
        transform.position = moveLocations[0];
        StartCoroutine(AppearanceAnimation());
    }
    private IEnumerator HitAnimation()
    {
        animator.SetInteger("state", 1);
        yield return new WaitForSeconds(1.5f);
        animator.SetInteger("state", 0);

        if (isTheFinalHit)
        {
            StartCoroutine(DieAnimation());
        }
        else
        {
            StartCoroutine(GetAngry());
        }

    }


    private IEnumerator GetAngry()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(2f);
        sprite.color = Color.white;
        isMoving = true;
        BossMoving.Invoke();
    }

    private IEnumerator AppearanceAnimation()
    {
        animator.SetInteger("state", 2);
        yield return new WaitForSeconds(2);
        animator.SetInteger("state", 0);
    }

    private IEnumerator DieAnimation()
    {
        animator.SetInteger("state", 3);
        yield return new WaitForSeconds(2);
        animator.SetInteger("state", 0);
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
