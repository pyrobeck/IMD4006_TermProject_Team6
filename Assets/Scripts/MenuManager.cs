using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject controlsPanel; 
    public GameObject creditsPanel;  
    public GameObject levelsPanel;  

    public AudioClip backgroundMusic; 
    private AudioSource musicSource;

    void Start()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);        
        levelsPanel.SetActive(false);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.05f; 

        musicSource.Play();
    }

    // Start Level 1
    public void StartGame()
    {
        StopMusic();
        SceneManager.LoadScene("Level_1");
    }

    // Open Levels Scene
    public void ShowLevels()
    {
        levelsPanel.SetActive(true);
    }

    public void HideLevels()
    {
        levelsPanel.SetActive(false);
    }

    // Show Controls Image
    public void ShowControls()
    {
        controlsPanel.SetActive(true);
    }

    public void HideControls()
    {
        controlsPanel.SetActive(false);
    }

    // Show Credits
    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }

    // Level buttons
    public void LoadLevel1()
    {
        StopMusic();
        Debug.Log("Load Level 1");
                SceneManager.LoadScene("Level_1");
    }

    public void LoadLevel2()
    {
        StopMusic();
        Debug.Log("Load Level 2");
        //SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3()
    {
        StopMusic();
        Debug.Log("Load Level 3");
        //SceneManager.LoadScene("Level3");
    }

    private void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}

//// credit to Vyra or MokkaMusic