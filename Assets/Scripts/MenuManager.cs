using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject controlsPanel; 
    public GameObject creditsPanel;  
    public GameObject levelsPanel;  
    public GameObject mainPanel;  

    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button levelsButton;
    public Button controlsButton;
    public Button creditsButton;

    [Header("Levels Menu Buttons")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button backFromLevelsButton;

    [Header("Controls Menu Buttons")]
    public Button backFromControlsButton;

    [Header("Credits Menu Buttons")]
    public Button backFromCreditsButton;

    private Button[] activeButtons;
    private int selectedIndex = 0;
    private float inputCooldown = 0.25f;
    private float lastInputTime = 0;

    private Color normalColor = Color.white;
    private Color highlightColor = Color.yellow;

    private bool justOpenedPanel = false;

    [Header("Audio")]
    public AudioClip backgroundMusic; 
    private AudioSource musicSource;

    void Start()
    {
        // Deactivate extra panels
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);        
        levelsPanel.SetActive(false);
        mainPanel.SetActive(true);

        // Main menu active by default
        SetActiveButtons(new Button[] { startButton, levelsButton, controlsButton, creditsButton });

        // Start background music
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.05f; 
        musicSource.Play();
    }

    void Update()
    {
        HandleJoystickNavigation();
        HandleSelection();
    }

    // ----------------------------
    // JOYSTICK MENU NAVIGATION
    // ----------------------------
    void HandleJoystickNavigation()
    {
        float vertical = Input.GetAxis("Vertical");

        if (Time.time - lastInputTime < inputCooldown) return;
        if (activeButtons == null || activeButtons.Length == 0) return;

        if (vertical > 0.5f)
        {
            selectedIndex--;
            if (selectedIndex < 0) selectedIndex = activeButtons.Length - 1;
            HighlightButton(selectedIndex);
            lastInputTime = Time.time;
        }
        else if (vertical < -0.5f)
        {
            selectedIndex++;
            if (selectedIndex >= activeButtons.Length) selectedIndex = 0;
            HighlightButton(selectedIndex);
            lastInputTime = Time.time;
        }
    }

    void HighlightButton(int index)
    {
        for (int i = 0; i < activeButtons.Length; i++)
        {
            ColorBlock cb = activeButtons[i].colors;

            if (i == index)
            {
                cb.normalColor = highlightColor;
                cb.selectedColor = highlightColor;
                cb.highlightedColor = highlightColor;
            }
            else
            {
                cb.normalColor = normalColor;
                cb.selectedColor = normalColor;
                cb.highlightedColor = normalColor;
            }

            activeButtons[i].colors = cb;
        }

        EventSystem.current.SetSelectedGameObject(activeButtons[index].gameObject);
    }

    // ----------------------------
    // SELECT BUTTON (ANY KEY)
    // ----------------------------
    void HandleSelection()
    {
        if (justOpenedPanel)
        {
            justOpenedPanel = false;
            return;
        }

        // 1 second cooldown between presses
        if (Time.time - lastInputTime < 1f) return;

        // ONLY trigger on the "Submit" (A, X, Enter)
        if (Input.GetButtonDown("Submit"))
        {
            activeButtons[selectedIndex].onClick.Invoke();
            lastInputTime = Time.time;
        }
    }


    private void SetActiveButtons(Button[] buttons)
    {
        activeButtons = buttons;
        selectedIndex = 0;
        HighlightButton(selectedIndex);

        justOpenedPanel = true;

        float extraDelay = 1.5f;  // ← set delay here
        lastInputTime = Time.time + extraDelay; // ← pushes input time forward
    }


    // ----------------------------
    // Button Actions
    // ----------------------------
    public void StartGame()
    {
        StopMusic();
        SceneManager.LoadScene("Level_1");
    }

    public void ShowLevels()
    {
        mainPanel.SetActive(false);
        levelsPanel.SetActive(true);

        SetActiveButtons(new Button[] { level1Button, level2Button, level3Button, backFromLevelsButton });
    }

    public void HideLevels()
    {
        levelsPanel.SetActive(false);
        mainPanel.SetActive(true);

        SetActiveButtons(new Button[] { startButton, levelsButton, controlsButton, creditsButton });
    }

    public void ShowControls()
    {
        mainPanel.SetActive(false);
        controlsPanel.SetActive(true);

        SetActiveButtons(new Button[] { backFromControlsButton });
    }

    public void HideControls()
    {
        controlsPanel.SetActive(false);
        mainPanel.SetActive(true);

        SetActiveButtons(new Button[] { startButton, levelsButton, controlsButton, creditsButton });
    }

    public void ShowCredits()
    {
        mainPanel.SetActive(false);
        creditsPanel.SetActive(true);

        SetActiveButtons(new Button[] { backFromCreditsButton });
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
        mainPanel.SetActive(true);

        SetActiveButtons(new Button[] { startButton, levelsButton, controlsButton, creditsButton });
    }

    public void LoadLevel1()
    {
        StopMusic();
        SceneManager.LoadScene("FirstComic");
    }

    public void LoadLevel2()
    {
        StopMusic();
        SceneManager.LoadScene("SecondComic");
    }

    public void LoadLevel3()
    {
        StopMusic();
        SceneManager.LoadScene("Level_3");
    }

    private void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
    }
}
