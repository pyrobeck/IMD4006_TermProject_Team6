using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Button_door : MonoBehaviour
{
    [SerializeField] private GameObject targetToDelete;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            //Debug.Log("well that would explain it");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ThrownObject"))
        {
            if (targetToDelete != null)
            {
                Destroy(targetToDelete);
                StartCoroutine(Spin());
            }
        }
    }

    private IEnumerator Spin()
    {
        animator.SetInteger("state", 1);
        yield return new WaitForSeconds(0.5f);
        animator.SetInteger("state", 2);
        //this can be deleted once we have an actual "activated" sprite/animation
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
