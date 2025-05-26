using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    int vertical;
    int horizontal;

    void Awake()
    {
        animator = GetComponent<Animator>(); 
        horizontal = Animator.StringToHash("Horizontal"); // Converts the string "Horizontal" to a hash for better performance.
        vertical = Animator.StringToHash("Vertical"); // Converts the string "Vertical" to a hash for better performance.
    }
    
    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting); // Sets the "isInteracting" boolean in the animator to control animation transitions.
        animator.CrossFade(targetAnim, 0.2f); // Crossfades to the target animation over 0.2 seconds.
    }

    public void PlayFallingAnimation(string targetAnim, bool isFalling)
    {
        animator.SetBool("isFalling", isFalling); // Sets the "isFalling" boolean in the animator to true.
        animator.CrossFade(targetAnim, 0.2f); // Crossfades to the target animation over 0.2 seconds.
    }
    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snapperdHorizontal;
        float snapperdVertical;

        #region Sanpperd Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f) // If the horizontal movement is greater than 0.5
        {
            snapperdHorizontal = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            snapperdHorizontal = 1f;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            snapperdHorizontal = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            snapperdHorizontal = -1f;
        }
        else
        {
            snapperdHorizontal = 0;
        }
        #endregion
        #region Snapperd Vertical

        if (verticalMovement > 0 && verticalMovement < 0.55f) // If the vertical movement is greater than 0.5
        {
            snapperdVertical = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            snapperdVertical = 1f;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snapperdVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            snapperdVertical = -1f;
        }
        else
        {
            snapperdVertical = 0;
        }
        #endregion

        if (isSprinting) // If the player is sprinting
        {
            snapperdHorizontal = horizontalMovement;
            snapperdVertical = 2; // Set vertical movement to 2
        }
        if(!animator.GetBool("isGrounded")) // If the player is interacting
        {
            snapperdHorizontal = 0; // Set horizontal movement to 0
            snapperdVertical = 0; // Set vertical movement to 0
        }
        animator.SetFloat(horizontal, snapperdHorizontal, 0.1f, Time.deltaTime); // Sets the horizontal movement value in the animator.
        animator.SetFloat(vertical, snapperdVertical, 0.1f, Time.deltaTime); // Sets the vertical movement value in the animator.
    }

}
