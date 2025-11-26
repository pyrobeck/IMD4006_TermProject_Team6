using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class WinTrigger : MonoBehaviour
{
    public GameObject winPanel;
    private InputSystem_Actions input;
    private bool canLoadScene = false;

    private void Awake()
    {
        input = new InputSystem_Actions();
    }

    private void Start()
    {
        winPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            winPanel.SetActive(true);
            canLoadScene = true;

            // Disable rolling animation on Player if needed:
            // other.GetComponent<Animator>().SetBool("Roll", false);
        }
    }

    private void OnEnable()
    {
        if (input == null)
            input = new InputSystem_Actions();

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
        if (!canLoadScene) return; // Only allow roll if win is active

        SceneManager.LoadScene("SecondComic"); // CHANGE to your scene name
    }
}
