using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public AnimationManager animManager;
    private PlayerControls input = null;
    public Vector2 inputVector2 = Vector2.zero;
    public Vector3 MOVE_DIRECTION; // STORES THE VALUE OF MOVEDIR SO ITS AVAILABLE TO OUTSIDE SCRIPTS
    public bool Can_Move = true; // IS USED BY THE ROLLSCRIPT TO FREEZE INPUT MOTION.
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AnimationCurve rollCurve;
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;

    public Transform cam;

    [TextArea(5, 10)]
    public string DebugTextArea;

    private void Awake()
    {
        input = new PlayerControls();
    }

    private void OnEnable()
    {
        input.Enable();
        input.PlayerMovement.Movement.performed += OnMovementPerformed; //HANDLES MOVEMENT COMMANDS
        input.PlayerMovement.Crouching.performed += OnCrouchTrigger;    //HANDLES CROUCHING BUTTONS
        input.PlayerMovement.Backstep.performed += OnBackstepTrigger;
        input.PlayerMovement.Attack.performed += OnAttackTrigger;
    }

    private void OnDisable()
    {
        input.Disable();
        input.PlayerMovement.Movement.performed -= OnMovementPerformed;
        input.PlayerMovement.Crouching.performed -= OnCrouchTrigger;
        input.PlayerMovement.Backstep.performed -= OnBackstepTrigger;
        input.PlayerMovement.Attack.performed -= OnAttackTrigger;


    }

    //FUNCTION IS CALLED EVERY TIME THE SYSTEM RECEIVES INPUT THAT HAS BEEN REGISTERED IN THE "PLAYERCONTROLS" INPUT ACTION MAP
    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        inputVector2 = value.ReadValue<Vector2>();
        animManager.manageWalk(inputVector2.magnitude);
    }

    private void OnCrouchTrigger(InputAction.CallbackContext value)
    {
        animManager.manageCrouch();
    }

    private void OnBlockTrigger(bool isPressed)
    {
        animManager.manageBlock(isPressed);
    }

    private void OnBackstepTrigger(InputAction.CallbackContext value)
    {
        if (inputVector2 == Vector2.zero)
        {
            // Debug.Log("Backstep pressed!");
        }
        else
        {
            // Debug.Log("Roll executed!");
            animManager.manageRoll();
        }
        // animManager.backStep();
    }

    private void OnAttackTrigger(InputAction.CallbackContext value){
        Debug.Log("Attack button pressed!");
        animManager.manageAttack();
    }



private IEnumerator Rollroutine(){
    Debug.Log("ROUTINE CALLED!");
    float t = 0;
    while(!Can_Move){
        Debug.Log("ROUTINE PLAYS!");
        transform.Translate(MOVE_DIRECTION.normalized * Time.deltaTime * rollCurve.Evaluate(t), Space.World);
        t += Time.deltaTime;
        yield return new WaitForEndOfFrame();
    }
}

public void startRoll(){
    StartCoroutine(Rollroutine());

}








    private void FixedUpdate()
    {
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

        
        if (Can_Move) //CHECKS IF THE CHARACTER IS ABLE TO MOVE (SET TO FALSE WHILE ROLLING)
        {
            MOVE_DIRECTION = moveDir.normalized;
            if (moveDir != Vector3.zero)
            {   //Handle smooth player rotation towards the target direction
                transform.forward = Vector3.Slerp(transform.forward, moveDir.normalized, Time.deltaTime * rotationSpeed);
            }
            transform.Translate(moveDir.normalized * Time.deltaTime * movementSpeed * inputVector2.magnitude, Space.World);
            animManager.setVelocity(inputVector2.magnitude);
        }
    }

    /// <summary>
    /// Callback for processing animation movements for modifying root motion.
    /// </summary>

    public Vector3 getMoveDirection() //RETURN THE CURRENT DIRECTION WE WISH TO MOVE TO
    {
        return MOVE_DIRECTION;
    }

    public void setMoveStatus(bool status) //CALLED BY THE ROLL SCRIPT
    {
        Can_Move = status;

    }
}
