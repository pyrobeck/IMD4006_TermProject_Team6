using UnityEngine;

public class starShooting : MonoBehaviour
{
    public GameObject note;
    public Transform notePos;
    public float bpm;

    private float timer;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        

        //only shoot when in range
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
        }

        
    }

    void shoot()
    {
        Instantiate(note, notePos.position, Quaternion.identity);
    }
}
