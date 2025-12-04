using UnityEngine;
using UnityEngine.Events;
using System.Collections;
public class bossDamage : MonoBehaviour
{
    public UnityEvent bossHit;
    public UnityEvent GameEnd;
    private int health = 2;
    private bool isInvincible = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ThrownObject") && isInvincible == false)
        {
            isInvincible = true;
            if (health == 0)
            {
                GameEnd.Invoke();
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
}
