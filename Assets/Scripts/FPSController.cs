using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{
    
    private bool canMove = true;
    
    [Header("Set Gravity")]
    // gravity 
    [SerializeField] private float gravity = 20.0f;

    [Header("Set Character Values")]
    // character speeds
    [SerializeField] private float walkSpeed = 7.0f;
    [SerializeField] private float runSpeed = 13.0f;
    [SerializeField] private float jumpSpeed = 8.5f;

    [Header("Set Camera Controls")]
    // camera controls
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float lookSpeed = 2.5f;
    [SerializeField] private float lookLimitX = 45.0f;

    
    // character controller
    CharacterController characterController;
    float rotationX = 0;
    Vector3 moveDirection = Vector3.zero;

    

    void Start()
    {
        // Locks mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCamera = Camera.main;
        characterController = GetComponent<CharacterController>();

    }

    void Update()
    {
        Vector3 moveForward = transform.TransformDirection(Vector3.forward);
        Vector3 moveRight = transform.TransformDirection(Vector3.right);
        
        
        // isRun to make player run
        
        bool isRun = Input.GetKey(KeyCode.LeftShift);
        
        float cursorSpeedX = canMove ? (isRun ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float cursorSpeedY = canMove ? (isRun ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        
        float movementDirectionY = moveDirection.y;
        moveDirection = (moveForward * cursorSpeedX) + (moveRight * cursorSpeedY);

        // jump 
        
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // gravity 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Moves the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Camera
        if (canMove)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            
            
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookLimitX, lookLimitX);
        }
    }
}