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

    public Transform cam;

    [TextArea(5,10)]
    public string DebugTextArea;

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
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = camRight.y = 0;

        Vector3 forwardRelative = inputVector2.y * camForward;
        Vector3 rightRelative = inputVector2.x * camRight;

        Vector3 moveDir = forwardRelative + rightRelative;

        DebugTextArea = $"Forward Relative: {forwardRelative}\n Right Relative: {rightRelative}\n Move Direction: {moveDir}";

        // rb.velocity = new Vector3(inputVector2.x, rb.velocity.y, inputVector2.y) * movementSpeed;
        // rb.velocity = new Vector3(moveDir.y, rb.velocity.y, moveDir.x) * movementSpeed;
        transform.Translate(moveDir.normalized * Time.deltaTime * movementSpeed, Space.World);
    }
    
}
