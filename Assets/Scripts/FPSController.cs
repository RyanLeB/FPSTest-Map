using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{
    [Header("Set Gravity")]
    // gravity 
    public float gravity = 20.0f;

    [Header("Set Character Values")]
    // character speeds
    public float walkSpeed = 7.0f;
    public float runSpeed = 13.0f;
    public float jumpSpeed = 8.5f;

    [Header("Set Camera Controls")]
    // camera controls
    public Camera playerCamera;
    public float lookSpeed = 2.5f;
    public float lookLimitX = 45.0f;

    
    // character controller
    CharacterController characterController;
    float rotationX = 0;
    Vector3 moveDirection = Vector3.zero;

    private bool canMove = true;
    

    void Start()
    {
        // Locks mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        characterController = GetComponent<CharacterController>();

    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        
        // isRunning bool to make player run
        
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // jump 
        
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

       
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Moves the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            
            
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookLimitX, lookLimitX);
        }
    }
}