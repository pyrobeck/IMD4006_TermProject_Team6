using UnityEngine;
using System.Collections;

public class audienceReactions : MonoBehaviour
{

    public AudioSource sfxSource;
    public AudioClip cheerSound;
    public AudioClip booSound;


    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Cheer()
    {
        animator.SetInteger("state", 1);
        if (sfxSource != null && cheerSound != null)
            sfxSource.PlayOneShot(cheerSound);
        StartCoroutine(ReturnToNeutral(4));
    }

    public void Boo()
    {
        animator.SetInteger("state", 2);
        sfxSource.PlayOneShot(booSound);
        StartCoroutine(ReturnToNeutral(7));
    }

    private IEnumerator ReturnToNeutral(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        animator.SetInteger("state", 0);
    }
}
