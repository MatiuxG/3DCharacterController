
using UnityEngine; // Imports the UnityEngine namespace for Unity-specific classes and functions.

public class InputManager : MonoBehaviour
{

    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    [SerializeField] Vector2 moveInput;
    [SerializeField] Vector2 cameraInput;
    [Header("Camera Sensitivity Settings")]
    [SerializeField] float cameraSensitivityX = 0.5f;
    [SerializeField] float cameraSensitivityY = 0.3f;

    [Header("Actions Managers")]

    public float moveAmount;

    public float cameraInputX;
    public float cameraInputY;
    public float verticalInput;
    public float horizontalInput;

    public bool SprintButtonPressed; // Boolean to check if the sprint button is pressed.
    public bool JumpButtonPressed; // Boolean to check if the jump button is pressed.

    public bool PauseButtonPressed; // Boolean to check if the pause button is pressed.

    void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>(); // Gets the AnimatorManager component attached to the same GameObject.
        playerLocomotion = GetComponent<PlayerLocomotion>(); // Gets the PlayerLocomotion component attached to the same GameObject.
    }

    void OnEnable()
    {
        if (playerControls == null) // Checks if playerControls has not been initialized.
        {
            playerControls = new PlayerControls(); // Creates a new instance of PlayerControls.

            // Subscribes to the Movement action's performed event to update moveInput when input is received.
            playerControls.PlayerMovement.Movement.performed += i => moveInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Sprint.performed += i => SprintButtonPressed = true; // Sets SprintButtonPressed to true when the sprint action is performed.
            playerControls.PlayerActions.Sprint.canceled += i => SprintButtonPressed = false; // Sets SprintButtonPressed to false when the sprint action is canceled.

            playerControls.PlayerActions.Jump.performed += i => JumpButtonPressed = true; // Sets JumpButtonPressed to true when the jump action is performed.

        }

        playerControls.Enable(); // Enables the playerControls input actions.
    }

    void OnDisable()
    {
        playerControls.Disable(); // Disables the playerControls input actions.
    }

    public void HandleAllInput()
    {
        HandleMovementInput(); // Calls the method to handle movement input.
        HandleSprintInput(); // Calls the method to handle sprint input.
        HandleJumpInput(); // Calls the method to handle jump input.
    }

    private void HandleMovementInput()
    {
        verticalInput = moveInput.y; // Gets the vertical input (up/down) from the moveInput vector.
        horizontalInput = moveInput.x; // Gets the horizontal input (left/right) from the moveInput vector.

        cameraInputX = cameraInput.x; // Gets the horizontal camera input (left/right) from the cameraInput vector.
        cameraInputY = cameraInput.y; // Gets the vertical camera input (up/down) from the cameraInput vector.

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)); // Clamps the moveAmount to a value between 0 and 1 based on the input.
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting); // Updates the animator values based on the input.

        verticalInput = moveInput.y;
        horizontalInput = moveInput.x;

        cameraInputX = cameraInput.x * cameraSensitivityX;
        cameraInputY = cameraInput.y * cameraSensitivityY;
    }

    private void HandleSprintInput()
    {
        if (SprintButtonPressed && moveAmount > 0.5f) // Checks if the sprint button is pressed and the player is moving.
        {
            playerLocomotion.isSprinting = true; // Sets isSprinting to true if the sprint button is pressed.
        }
        else
        {
            playerLocomotion.isSprinting = false; // Sets isSprinting to false if the sprint button is not pressed.
        }
    }

    public void HandleJumpInput()
    {
        if (JumpButtonPressed) // Checks if the jump button is pressed.
        {
            JumpButtonPressed = false; // Resets JumpButtonPressed to false after handling the jump input.
            playerLocomotion.HandleJumping(); // Calls the HandleJumping method in PlayerLocomotion to perform the jump action.
        }
    }

    public Vector2 GetCameraDelta()
    {
        return cameraInput; // Returns the camera input vector, which contains the delta values for camera movement.
    }

}
