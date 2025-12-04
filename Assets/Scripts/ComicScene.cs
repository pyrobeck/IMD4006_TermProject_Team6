using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class ComicSceneManager : MonoBehaviour
{
    public string[] allowedScenes; 
    public string[] firstComicScene;  

    private InputAction anyInputAction;

    private void Awake()
    {
        anyInputAction = new InputAction(
            type: InputActionType.Button,
            binding: "*/<Button>", 
            interactions: "press"
        );

        anyInputAction.performed += OnAnyInput;
    }

    private void OnEnable()
    {
        anyInputAction.Enable();
    }

    private void OnDisable()
    {
        anyInputAction.Disable();
    }

    private void OnAnyInput(InputAction.CallbackContext ctx)
    {
        TryAdvanceScene();
    }

    private void TryAdvanceScene()
    {
        string current = SceneManager.GetActiveScene().name;

        foreach (string allowed in allowedScenes)
        {
            if (current == allowed)
            {
                if (Array.Exists(firstComicScene, scene => scene == current))
                {
                    StartCoroutine(WaitAndLoadNextScene());
                    return;
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    return;
                }
            }
        }
    }

    private IEnumerator WaitAndLoadNextScene()
    {
        yield return new WaitForSeconds(2f);  
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
