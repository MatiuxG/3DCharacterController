using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string isInteractingBoll;
    public bool isInteractingStatus;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBoll, isInteractingStatus); // Sets the "isInteracting" boolean in the animator to control animation transitions.
    }
}
