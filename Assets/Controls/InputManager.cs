using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
 
    private PlayerControls input = null;
    public Vector2 inputVector2 = Vector2.zero;
    [SerializeField] private Rigidbody rb;
    [SerializeField] float movementSpeed;

    private void Awake() {
        input = new PlayerControls();
    }

    private void OnEnable() {
        input.Enable();
        input.PlayerMovement.Movement.performed += OnMovementPerformed;
    }

    private void OnDisable() {
        input.Disable();
        input.PlayerMovement.Movement.performed -= OnMovementPerformed;
        
    }

    //FUNCTION IS CALLED EVERY TIME THE SYSTEM RECEIVES INPUT THAT HAS BEEN REGISTERED IN THE "PLAYERCONTROLS" INPUT ACTION MAP
    private void OnMovementPerformed(InputAction.CallbackContext value){
        inputVector2 = value.ReadValue<Vector2>();
    }



    private void FixedUpdate() {

        //NEEDS EDIT. INPUT RECEIVES X,Y AND WE MAP TO THE X, Z VALUES OF VELOCITY. LOOK FOR SHORTER, MORE DIRECT WAY.
        rb.velocity = new Vector3(inputVector2.x, rb.velocity.y, inputVector2.y) * movementSpeed;
    }
    
}
