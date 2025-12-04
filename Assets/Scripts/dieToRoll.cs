using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Audio;

public class dieToRoll : MonoBehaviour
{
    private Animator animator;
    public string deathAnimName;
    public AudioClip explosion;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Die()
    {
        StartCoroutine(waitDie());
    }

    private IEnumerator waitDie()
    {
        if (audioSource != null)
            audioSource.PlayOneShot(explosion, 0.5f);
        

        animator.Play(deathAnimName);
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject, 0.1f);

    }

}
