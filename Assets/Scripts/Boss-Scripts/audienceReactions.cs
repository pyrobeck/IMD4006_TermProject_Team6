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
        StartCoroutine(ReturnToNeutral());
    }

    private IEnumerator ReturnToNeutral()
    {
        yield return new WaitForSeconds(4);
        animator.SetInteger("state", 0);
    }
}
