using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollScript : MonoBehaviour
{
    //THE SCRIPT ATTACHED TO THE ROLL ANIMATION SO ANIMATION EVENTS CAN HAPPEN
    public InputManager masterScript;
    
    public void motionOff(){
        masterScript.setMoveStatus(false);
    }
    public void motionOn(){
        masterScript.setMoveStatus(true);
    }

    public void beginRoll(){
        masterScript.startRoll();
    }
}
