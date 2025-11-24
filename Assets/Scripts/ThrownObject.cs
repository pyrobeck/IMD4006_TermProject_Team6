using UnityEngine;

public class HeldObject : MonoBehaviour
{

    void Start()
    {
        int direction = 1;
        if (transform.parent.localScale.x < 0)
        {
            direction = -1;
        }
        transform.parent = null;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(900 * direction, 350));
        Destroy(gameObject, 10); //destroy after 10 seconds in case it never hits anything somehow
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
