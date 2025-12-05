using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class bossDamage : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip screechSound;
    public AudioClip sadSound;

    public GameObject winPanel;

    public UnityEvent bossHit;
    public UnityEvent GameEnd;

    private int health = 2;
    private bool isInvincible = false;

    private void Start()
    {
        winPanel.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ThrownObject") && isInvincible == false)
        {
            // play screech
            if (sfxSource != null && screechSound != null)
            {
                sfxSource.volume = 1.0f;
                sfxSource.PlayOneShot(screechSound);
            }

            // boss takes damage
            health--;
            bossHit.Invoke();

            // check if dead
            Debug.Log(health);

            if (health <= -2)
            {
                GameEnd.Invoke();
                winPanel.SetActive(true);
                StartCoroutine(PlayAndFadeViolin());
                Debug.Log("dead");
            }

            // invincibility
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
