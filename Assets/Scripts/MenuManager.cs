using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject controlsPanel; // Assign your controls image panel here
    public GameObject creditsPanel;  // Assign a panel for credits text later

    // Start Level 1
    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); // Replace with your level 1 scene name
    }

    // Open Levels Scene
    public void OpenLevels()
    {
        SceneManager.LoadScene("LevelsScene"); // Make this another scene
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

}
