using System.Collections;
using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    void Start()
    {
        StartCoroutine(WaitAndBlink());
    }

    private IEnumerator WaitAndBlink()
    {
        while (true)
        {
            text.alpha = text.alpha > 0 ? 0 : 1;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
