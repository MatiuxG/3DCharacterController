using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    PlayerControls playerControls;
    [Header("UI Elements")]
    [SerializeField] private GameObject PauseCanvas; // The canvas that contains the pause menu UI elements.
    [SerializeField] private CursorControler cursorControler; // Reference to the CursorControler for managing cursor visibility and lock state.
    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Enable(); // Enable the player controls to listen for input.
        playerControls.PlayerActions.Pause.performed += ctx => TogglePause(); // Subscribe to the pause action input.
    }
    void Start()
    {
        cursorControler = GetComponent<CursorControler>(); // Get the CursorControler component attached to the same GameObject.
        PauseCanvas.SetActive(false); // Initially hide the pause menu canvas.
        
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0) // If the game is paused
        {
            Time.timeScale = 1; // Resume the game
        }
        else // If the game is running
        {

            Time.timeScale = 0; // Pause the game
        }
        cursorControler.UpdateCursorState(); // Update the cursor state to be visible and unlocked
        if (PauseCanvas != null)
        {
            PauseCanvas.SetActive(!PauseCanvas.activeSelf); // Toggle the visibility of the pause menu canvas.
        }
    }
}
