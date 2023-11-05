using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
 
    public AnimationManager animManager;
    private PlayerControls input = null;
    public Vector2 inputVector2 = Vector2.zero;
    [SerializeField] private Rigidbody rb;
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;

    public Transform cam;

    [TextArea(5,10)]
    public string DebugTextArea;

    private void Awake() {
        input = new PlayerControls();
    }

    private void OnEnable() {
        input.Enable();
        input.PlayerMovement.Movement.performed += OnMovementPerformed; //HANDLES MOVEMENT COMMANDS
        input.PlayerMovement.Crouching.performed += OnCrouchTrigger;    //HANDLES CROUCHING BUTTONS
        input.PlayerMovement.Backstep.performed += OnBackstepTrigger;
    }

    private void OnDisable() {
        input.Disable();
        input.PlayerMovement.Movement.performed -= OnMovementPerformed;
        input.PlayerMovement.Crouching.performed -= OnCrouchTrigger;
        input.PlayerMovement.Backstep.performed -= OnBackstepTrigger;
        
        
    }

    //FUNCTION IS CALLED EVERY TIME THE SYSTEM RECEIVES INPUT THAT HAS BEEN REGISTERED IN THE "PLAYERCONTROLS" INPUT ACTION MAP
    private void OnMovementPerformed(InputAction.CallbackContext value){
        inputVector2 = value.ReadValue<Vector2>();
        animManager.manageWalk(inputVector2.magnitude);
    }

    private void OnCrouchTrigger(InputAction.CallbackContext value){
        animManager.manageCrouch();
    }

    private void OnBlockTrigger(bool isPressed){
        animManager.manageBlock(isPressed);
    }

    private void OnBackstepTrigger(InputAction.CallbackContext value){
        Debug.Log("Backstep pressed!");
        // animManager.backStep();
    }



    private void FixedUpdate() {
        //CHECK IF BLOCK BUTTON IS PRESSED AND PERFORM ACCORDINGLY
        OnBlockTrigger(input.PlayerMovement.Block.IsPressed());

        //NEEDS EDIT. INPUT RECEIVES X,Y AND WE MAP TO THE X, Z VALUES OF VELOCITY. LOOK FOR SHORTER, MORE DIRECT WAY.
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = camRight.y = 0;

        Vector3 forwardRelative = inputVector2.y * camForward;
        Vector3 rightRelative = inputVector2.x * camRight;

        Vector3 moveDir = forwardRelative + rightRelative;

        DebugTextArea = $"Input value: {inputVector2.magnitude}";
        // DebugTextArea = $"Forward Relative: {forwardRelative}\n Right Relative: {rightRelative}\n Move Direction: {moveDir}";

        // rb.velocity = new Vector3(inputVector2.x, rb.velocity.y, inputVector2.y) * movementSpeed;
        // rb.velocity = new Vector3(moveDir.y, rb.velocity.y, moveDir.x) * movementSpeed;

        //Handle smooth player rotation towards the target direction
        if(moveDir != Vector3.zero){
            transform.forward = Vector3.Slerp(transform.forward, moveDir.normalized, Time.deltaTime * rotationSpeed);
        }
        transform.Translate(moveDir.normalized * Time.deltaTime * movementSpeed, Space.World);
    }

    /// <summary>
    /// Callback for processing animation movements for modifying root motion.
    /// </summary>
    
    
}
