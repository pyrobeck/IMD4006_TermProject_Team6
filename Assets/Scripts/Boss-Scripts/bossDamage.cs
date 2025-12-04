using UnityEngine;
using UnityEngine.Events;
using System.Collections;
public class bossDamage : MonoBehaviour
{
    public UnityEvent bossHit;
    private int health = 3;
    private bool isInvincible = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ThrownObject") && isInvincible == false)
        {
            Debug.Log("I'VE BEEN HIT!!!");
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
