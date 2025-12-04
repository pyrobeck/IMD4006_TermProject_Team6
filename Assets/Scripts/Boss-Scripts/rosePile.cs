using UnityEngine;
using System.Collections;
public class rosePile : MonoBehaviour
{
    [SerializeField] private Vector2 targetPosition;

    public void Appear()
    {
        StartCoroutine(AppearAfterAMoment());
    }

    private IEnumerator AppearAfterAMoment()
    {
        yield return new WaitForSeconds(1.5f);
        transform.position = targetPosition;
    }
}
