using UnityEngine;
using UnityEngine.Events;
using System.Collections;
public class bossDamage : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip screechSound;
    public AudioClip sadSound;

    public UnityEvent bossHit;
    public UnityEvent GameEnd;
    private int health = 2;
    private bool isInvincible = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ThrownObject") && isInvincible == false)
        {
            if (sfxSource != null && screechSound != null)
                sfxSource.volume = 2.0f; // 50% volume
                sfxSource.PlayOneShot(screechSound);

            isInvincible = true;
            if (health == 0)
            {
                GameEnd.Invoke();
                StartCoroutine(PlayAndFadeViolin());
                Debug.Log("dead");
            }
            bossHit.Invoke();
            health--;

            isInvincible = true;
            StartCoroutine(Invincibility());
        }
    }


    private IEnumerator Invincibility()
    {
        yield return new WaitForSeconds(5);
        isInvincible = false;
    }

    private IEnumerator PlayAndFadeViolin()
    {
        sfxSource.clip = sadSound;
        sfxSource.volume = 0.5f;
        sfxSource.Play();

        float duration = 5f;
        float startVolume = 0.5f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            sfxSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        sfxSource.volume = 0f;
        sfxSource.Stop();
    }

}
