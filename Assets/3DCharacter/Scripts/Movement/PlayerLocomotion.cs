using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [Header("References")]
    InputManager inputManager; // Reference to the InputManager for handling player input.
    Vector3 moveDirection;
    Transform cameraObject;
    PlayerManager playerManager; // Reference to the PlayerManager for handling player interactions.
    AnimatorManager animatorManager; // Reference to the AnimatorManager for handling animations.
    [SerializeField] Rigidbody rb;

    [Header("Ground Check")]
    [SerializeField] Transform feets; // Punto ajustable desde el inspector para la detección del suelo


    [Header("Falling")]
    [SerializeField] float inAirTimer; // Timer to check if the player is in the air.
    [SerializeField] float leapingVelocity; // Velocity of the player when jumping.
    [SerializeField] float fallingVelocity; // Speed of the player falling.
    [SerializeField] LayerMask groundLayer; // Layer mask to check for ground collisions.
    [SerializeField] float rayCastHeightOffSet = 0.5f; // Height offset for the raycast to check for ground collisions.
    [SerializeField] float maxDistance = .01f; // Maximum distance for the raycast to check for ground collisions.


    [Header("Walking and Running Variables")]
    [SerializeField] float runningSpeed = 5f; // Speed of the player movement.
    [SerializeField] float walkingSpeed = 1.5f; // Speed of the player walking.
    [SerializeField] float sprintingSpeed = 7f; // Speed of the player sprinting.
    [SerializeField] float rotationSpeed = 15f; // Speed of the player movement.

    [Header("Jumping Variables")]
    [SerializeField] float jumpHeight = 1.5f; // Height of the player's jump.
    [SerializeField] float gravityIntensity = 9.81f; // Gravity intensity for the player's jump.


    [Header("Movement Flags")]
    public bool isSprinting; // Boolean to check if the player is sprinting.
    public bool isGrounded; // Boolean to check if the player is grounded.
    public bool isJumping; // Boolean to check if the player is jumping.
    public bool isCrouching; // Boolean to check if the player is crouching.


    [Header("Movement Conditions")]
    [Tooltip("Can the player move while jumping? or While falling?")]
    [SerializeField] bool canMoveWhileJumping; // Boolean to check if the player can move while jumping.
    [SerializeField] bool canMoveWhileFalling; // Boolean to check if the player can move while falling.

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>(); // Gets the PlayerManager component attached to the same GameObject.
        inputManager = GetComponent<InputManager>(); // Gets the InputManager component attached to the same GameObject.
        rb = GetComponent<Rigidbody>(); // Gets the Rigidbody component attached to the same GameObject.
        cameraObject = Camera.main.transform; // Gets the main camera's transform to use for player rotation.
        animatorManager = GetComponent<AnimatorManager>(); // Gets the AnimatorManager component attached to the same GameObject.
    }

    public void HandleAllMovement()
    {
        if (playerManager.isInteracting) // Checks if the player is interacting with something.
        {
            return; // If interacting, exit the method to prevent movement.
        }
        HandleMovement(); // Calls the method to handle player movement.
        HandleRotation(); // Calls the method to handle player rotation.
        ApplyExtraFallingForce();
    }
    private void HandleMovement()
    {
        if (!canMoveWhileFalling && !isGrounded)
        {
            return; // If the player is falling, exit the method to prevent movement.
        }

        if (!canMoveWhileJumping && isJumping)
        {
            return; // If the player is jumping, exit the method to prevent rotation.
        }
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize(); // Normalizes the moveDirection vector to ensure consistent speed in all directions.
        moveDirection.y = 0; // Sets the y component to 0 to prevent vertical movement.

        if (isSprinting)
        {
            moveDirection = moveDirection * sprintingSpeed; // Multiplies the moveDirection by the sprinting speed to control the speed of the player.
        }

        else
        {
            if (inputManager.moveAmount >= 0.5f) // Checks if the player is moving.
            {

                moveDirection = moveDirection * runningSpeed; // Multiplies the moveDirection by the running speed to control the speed of the player.
            }
            else
            {
                moveDirection = moveDirection * walkingSpeed; // If not moving, set the movement speed to 0.
            }

        }


        Vector3 moveVelocity = moveDirection;
        rb.linearVelocity = moveVelocity;
    }

    private void HandleRotation()
    {
        if (isJumping)
        {
            return; // If the player is jumping, exit the method to prevent rotation.
        }

        if (!isGrounded)
        {
            return; // If the player is falling, exit the method to prevent rotation.
        }
        // Handle rotation logic here
        Vector3 targetDirection = Vector3.zero; // Initialize targetDirection to zero

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize(); // Normalize the target direction to ensure consistent rotation speed.
        targetDirection.y = 0; // Set the y component to zero to prevent vertical rotation.

        if (targetDirection == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection); // Create a rotation based on the target direction.
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Smoothly interpolate to the new rotation.

        transform.rotation = playerRotation; // Apply the new rotation to the player.
    }


    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true); // Set the isJumping boolean in the animator to true.
            animatorManager.PlayTargetAnimation("Jump", true); // Play jump animation.

            float jumpForce = Mathf.Sqrt(-2 * -gravityIntensity * jumpHeight); // Set the jump force.
            Vector3 playerVelocity = moveDirection; // Create a jump vector.
            playerVelocity.y = jumpForce; // Set the y component of the jump vector to the jump force.
            rb.linearVelocity = playerVelocity; // Apply the jump vector to the player's velocity.
        }
    }


  public void SetIsGrounded(bool value)
{
    bool wasGrounded = isGrounded;
    isGrounded = value;

    if (isGrounded)
        isJumping = false; 

    animatorManager.animator.SetBool("isGrounded", isGrounded);
    animatorManager.animator.SetBool("isFalling", !isGrounded && !isJumping);

    if (!wasGrounded && isGrounded && !isJumping)
    {
        animatorManager.PlayFallingAnimation("Landing", false);
        inAirTimer = 0f;
    }

    if (!isGrounded && !isJumping && !playerManager.isFalling)
    {
        animatorManager.PlayFallingAnimation("Falling", true);
    }
}



    private void ApplyExtraFallingForce()
    {
        if (!isGrounded && !isJumping)
        {
            inAirTimer += Time.deltaTime;
            rb.AddForce(Vector3.down * fallingVelocity * inAirTimer, ForceMode.Acceleration);
        }
    }

    private HashSet<Collider> groundColliders = new HashSet<Collider>();

    public void RegisterGroundContact(Collider collider)
    {
        if (!groundColliders.Contains(collider))
        {
            groundColliders.Add(collider);
            SetIsGrounded(true);
            Debug.Log($"[PlayerLocomotion] Ground contact registered: {collider.name}");
        }
    }

    public void UnregisterGroundContact(Collider collider)
{
    if (groundColliders.Contains(collider))
    {
        groundColliders.Remove(collider);
        Debug.Log($"[PlayerLocomotion] Ground contact removed: {collider.name}");
    }

    if (groundColliders.Count == 0)
    {
        // Retardo para estabilidad
        CancelInvoke(nameof(DelayGroundedOff));
        Invoke(nameof(DelayGroundedOff), 0.1f);
    }
}

    private void DelayGroundedOff()
    {
        if (groundColliders.Count == 0)
            SetIsGrounded(false);
    }
    


}
