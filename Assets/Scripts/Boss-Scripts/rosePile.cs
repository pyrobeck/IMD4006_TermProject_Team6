using UnityEngine;
using System.Collections;
public class rosePile : MonoBehaviour
{
    [SerializeField] private Vector2[] targetPositions;
    private int currentSection = 0;
    private Vector2 hiddenPosition;

    void Start()
    {
        hiddenPosition = transform.position;
    }

    public void Appear()
    {
        StartCoroutine(AppearAfterAMoment());
    }

    private IEnumerator AppearAfterAMoment()
    {
        yield return new WaitForSeconds(1.5f);
        transform.position = targetPositions[currentSection];
    }

    public void Dissapear()
    {
        transform.position = hiddenPosition;
        currentSection++;
    }

}
