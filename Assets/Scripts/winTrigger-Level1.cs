using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    public GameObject winPanel; // drag UI panel here

    private void Start()
    {
        winPanel.SetActive(false); // hide on start
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            winPanel.SetActive(true);
            //Time.timeScale = 0f; // optional: pause the game
        }
    }

    // This is called by the UI button
    public void LoadNextScene()
    {
        //Time.timeScale = 1f; // unpause
        SceneManager.LoadScene("SecondComic");
    }
}
