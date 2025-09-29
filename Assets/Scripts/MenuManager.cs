using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject controlsPanel; // Assign your controls image panel here
    public GameObject creditsPanel;  // Assign a panel for credits text later
    public GameObject levelsPanel;  

    void Start()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);        
        levelsPanel.SetActive(false);

    }

    // Start Level 1
    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); // Replace with your level 1 scene name
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
        Debug.Log("Load Level 1");
        //SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        Debug.Log("Load Level 2");
        //SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3()
    {
        Debug.Log("Load Level 3");
        //SceneManager.LoadScene("Level3");
    }
}
