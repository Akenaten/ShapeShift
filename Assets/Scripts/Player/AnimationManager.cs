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

    // public void backStep(){
    //     animator.SetTrigger("Backstep");
    // }

    
    // void OnAnimatorMove()
    // {
    //     transform.parent.position +=animator.deltaPosition;
    // }
}
