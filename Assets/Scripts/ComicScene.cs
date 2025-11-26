using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ComicSceneManager : MonoBehaviour
{
    public string[] allowedScenes;   

    private InputSystem_Actions input;

    private void Awake()
    {
        input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Roll.performed += OnRollPressed;
    }

    private void OnDisable()
    {
        input.Player.Roll.performed -= OnRollPressed;
        input.Player.Disable();
    }

    private void OnRollPressed(InputAction.CallbackContext ctx)
    {
        string current = SceneManager.GetActiveScene().name;

        foreach (string allowed in allowedScenes)
        {
            if (current == allowed)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return;
            }
        }
    }
}
