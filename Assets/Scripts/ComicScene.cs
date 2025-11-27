using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ComicSceneManager : MonoBehaviour
{
    public string[] allowedScenes;

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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return;
            }
        }
    }
}
