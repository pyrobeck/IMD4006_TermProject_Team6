using UnityEngine;
using System.Collections;

public class audienceReactions : MonoBehaviour
{

    public AudioSource sfxSource;
    public AudioSource chatterSource;

    public AudioClip cheerSound;
    public AudioClip booSound;
    public AudioClip chatterSound;



    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
            if (chatterSource != null && chatterSound != null){
                chatterSource.volume = 0.2f; // 50% volume
                chatterSource.PlayOneShot(chatterSound);
            }
    }

    public void Cheer()
    {
        animator.SetInteger("state", 1);
        if (sfxSource != null && cheerSound != null){
            sfxSource.volume = 0.2f; // 50% volume
            sfxSource.PlayOneShot(cheerSound);
        }
        StartCoroutine(ReturnToNeutral(4));
    }

    public void Boo()
    {
        Debug.Log("BOO CALLED");

        animator.SetInteger("state", 2);
        sfxSource.volume = 1.0f; // 50% volume
        sfxSource.PlayOneShot(booSound);
        StartCoroutine(ReturnToNeutral(7));
    }

    private IEnumerator ReturnToNeutral(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        animator.SetInteger("state", 0);
    }
}
