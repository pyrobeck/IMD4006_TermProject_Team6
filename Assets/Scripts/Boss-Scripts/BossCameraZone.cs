using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BossCameraZone : MonoBehaviour
{

    [SerializeField] private Vector2[] cameraLockPositions;
    [SerializeField] private Vector2[] cameraZonePositions;
    [SerializeField] private cameraMovement camera;
    private int bossSection = 0;
    private bool hasEnteredBefore = false;
    public UnityEvent BeginBossSection;

    void Start()
    {
        transform.position = cameraZonePositions[bossSection];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (hasEnteredBefore == false)
            {
                Debug.Log("Starting Boss Section");
                hasEnteredBefore = true;
                StartCoroutine(BeginSection());
            }
            camera.LockCamera(cameraLockPositions[bossSection]);
        }
    }

    private IEnumerator BeginSection()
    {
        yield return new WaitForSeconds(1.5f);
        BeginBossSection.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            camera.UnlockCamera();
        }
    }

    public void LeaveForBoss()
    {
        bossSection++;
        transform.position = cameraZonePositions[bossSection];
    }
}


