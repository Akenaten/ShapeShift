using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator animator;
    public InputManager parentScript;
    
    //MANAGES THE WALKING BOOLEAN OF THE ANIMATOR
    public void manageWalk(float value){
        if(value > 0.1){
            animator.SetBool("Walking", true);
        } 
        else {
            animator.SetBool("Walking", false);
        }

    }

    //MANAGES THE CROUCHING BOOLEAN OF THE ANIMATOR
    public void manageCrouch(){
        animator.SetBool("Crouching", (animator.GetBool("Crouching") ? false : true));
    }

    public void manageBlock(bool state){
        animator.SetBool("Blocking", state);

    }

// BACKSTEP HAS ROOT MOTION AND GLIDES BACK TO PREVIOUS POSITION AFTERWARDS.
    public void backStep(){
        animator.SetTrigger("Backstep");
    }

    public void manageRoll(){
        animator.SetTrigger("Roll");
    }

    public void manageAttack(){
        animator.SetTrigger("Attack");
    }

    public void setVelocity(float currentVelocity){
        currentVelocity = (currentVelocity > 1) ? 1 : currentVelocity;
        animator.SetFloat("Velocity", currentVelocity);
    }

    
    // void OnAnimatorMove()
    // {
    //     transform.parent.position +=animator.deltaPosition;
    // }
}
