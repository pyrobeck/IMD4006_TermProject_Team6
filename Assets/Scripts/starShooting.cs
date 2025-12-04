using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class starShooting : MonoBehaviour
{
    public GameObject note;
    public Transform notePos;
    public float bpm;
    public AudioSource starSound;

    private float timer;
    private GameObject player;

    [SerializeField] bpmCounter bpmCounter;

    private bool startUpPanic = true;
    private float lastBeatTime;
    private bool yesShoot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lastBeatTime = bpmCounter.GetSongPosition();

        StartCoroutine(WaitForSongPositionFreakOutToEnd());
    }

    // Update is called once per frame
    void Update()
    {

        if (startUpPanic == true)
        {
            return;
        }

        UpdateCurrentBeat();


        /*//only shoot when in range
        float distance = Vector2.Distance(transform.position, player.transform.position);
        //Debug.Log(distance);

        if(distance < 8)
        {
            timer += Time.deltaTime;

            //time between bullets
            if (timer > bpm)
            {
                
                timer = 0;

                shoot();
            }
        }*/




    }

    void shoot()
    {
        if (starSound != null)
        {
            starSound.Play();
        }

        StartCoroutine(soundWait());



    }

    private void CheckCurrentBeat(int beatNumber)
    {

        if (bpmCounter.GetCurrentBeat() == beatNumber)
        {
            
            yesShoot = !yesShoot;

            if (yesShoot)
            {
                //only shoot when in range
                float distance = Vector2.Distance(transform.position, player.transform.position);
                Debug.Log(distance);

                if (distance < 8)
                {
                    shoot();
                }

            }
            
        }
    }

    private void UpdateCurrentBeat()
    {
        if (bpmCounter.GetSongPosition() < (lastBeatTime + bpmCounter.GetTimeBetweenBeats()))
        {
            return;
        }

        lastBeatTime += bpmCounter.GetTimeBetweenBeats();


        CheckCurrentBeat(4);

    }


    private IEnumerator WaitForSongPositionFreakOutToEnd()
    {
        yield return new WaitForSeconds(2);
        lastBeatTime = bpmCounter.GetSongPosition();
        startUpPanic = false;
    }

    private IEnumerator soundWait()
    {
        yield return new WaitForSeconds(0.9f);


        Instantiate(note, notePos.position, Quaternion.identity);

    }
}


