using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager; // Reference to the InputManager for handling player input.
    PlayerLocomotion playerLocomotion; // Reference to the PlayerLocomotion for handling player movement and rotation.
    Animator animator; // Reference to the AnimatorManager for handling animations.


    public bool isInteracting; // Boolean to check if the player is interacting with something.

    public bool isFalling; // Boolean to check if the player is jumping.
    void Awake()
    {
        inputManager = GetComponent<InputManager>(); // Gets the InputManager component attached to the same GameObject.
        playerLocomotion = GetComponent<PlayerLocomotion>(); // Gets the PlayerLocomotion component attached to the same GameObject.

        animator = GetComponent<Animator>(); // Gets the AnimatorManager component attached to the same GameObject.
    }

    void Update()
    {
        if(Time.timeScale == 0) // Checks if the game is paused.
        {
            return; // If paused, exit the method to prevent movement.
        }
        inputManager.HandleAllInput(); // Calls the method to handle all input from the InputManager.
    }

    void FixedUpdate()
    {
        if(Time.timeScale == 0) // Checks if the game is paused.
        {
            return; // If paused, exit the method to prevent movement.
        }
        playerLocomotion.HandleAllMovement(); // Calls the method to handle all movement from the PlayerLocomotion.
    }

    void LateUpdate()
    {
        isInteracting = animator.GetBool("isInteracting"); // Gets the value of the "isInteracting" boolean from the animator to check if the player is interacting with something.
        isFalling = animator.GetBool("isFalling"); // Gets the value of the "isFalling" boolean from the animator to check if the player is falling.
        animator.SetBool("isGrounded", playerLocomotion.isGrounded); // Sets the "isGrounded" boolean in the animator to control animation transitions.
        animator.SetBool("isJumping", playerLocomotion.isJumping); // Sets the "isJumping" boolean in the animator to control animation transitions.
    }
}
