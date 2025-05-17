using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartListener : MonoBehaviour
{
    private InputAction restartAction;

    private void Awake()
    {
        restartAction = new InputAction("Restart", InputActionType.Button, "<Keyboard>/r");
    }

    private void OnEnable()
    {
        restartAction.Enable();
        restartAction.performed += OnRestart;
    }

    private void OnDisable()
    {
        restartAction.performed -= OnRestart;
        restartAction.Disable();
    }

    private void OnRestart(InputAction.CallbackContext context)
    {
        Debug.Log("Restarting Scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
