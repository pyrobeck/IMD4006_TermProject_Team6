using UnityEngine;
using System.Collections;

public class audienceReactions : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Cheer()
    {
        animator.SetInteger("state", 1);
        StartCoroutine(ReturnToNeutral(4));
    }

    public void Boo()
    {
        animator.SetInteger("state", 2);
        StartCoroutine(ReturnToNeutral(7));
    }

    private IEnumerator ReturnToNeutral(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        animator.SetInteger("state", 0);
    }
}
