using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float runAcceleration = .25f;
    [SerializeField] private float runSpeed = 4f;
    
    [Header("Movement")]

    [SerializeField] private float groundDrag;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private bool readyToJump;
    [SerializeField] private float wallRunSpeed;
    
    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    bool grounded;

    [SerializeField] private Transform orientation;

    float horizontalInput;
    float verticalInput;
    

    private Vector2 moveInputValue;
    Vector3 moveDirection;

    private PLAYER_STATES currenPlayerState;
    public bool wallrunning;



    private void SubscribeToPlayerState()
    {
        PlayerStates.GetInstance().OnPlayerStateChanged += PlayerStateChange;
        PlayerStateChange(PlayerStates.GetInstance().GetCurrentPlayerState());
    }

    private void PlayerStateChange(PLAYER_STATES _newPlayerStates)
    {
        
        currenPlayerState = _newPlayerStates;
        
        switch (_newPlayerStates)
        {
            case PLAYER_STATES.WALKING:
                wallrunning = false;
                break;
            case PLAYER_STATES.WALLRUNNING:
                wallrunning = true;
                break;
        }
    }
    

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        
        ResetJump();
    }

    private void Start()
    {
        currenPlayerState = PlayerStates.GetInstance().GetCurrentPlayerState();
        SubscribeToPlayerState();

    }

    private void Update()
    {
        Debug.Log("En Third Person controller el estado es: " + PlayerStates.GetInstance().GetCurrentPlayerState());
        MyInput();
        SpeedControl();
        StateHandler();
    }

    private void FixedUpdate()
    {
        Vector3 raycastOrigin = transform.position + Vector3.up * (playerHeight * 0.5f);
        Debug.DrawRay(raycastOrigin, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.red);
        grounded = Physics.Raycast(raycastOrigin, Vector3.down, playerHeight * 0.5f +.2f, whatIsGround);


        if (grounded)
        {
            rigidBody.drag = groundDrag;
        }
        else
        {
            rigidBody.drag = 0f;
        }
        
        MovePlayer();
        
    }

    private void MyInput()
    {
        horizontalInput = InputManager.GetInstance().MovementInput().x;
        verticalInput = InputManager.GetInstance().MovementInput().y;

        if (InputManager.GetInstance().JumpInput() && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }


    private void StateHandler()
    {
        if (wallrunning)
        {
            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.WALLRUNNING);
            runSpeed = wallRunSpeed;
        }
        else
        {
            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.WALKING);
            runSpeed = 7;
        }
    }
    

    private void MovePlayer()
    {

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        rigidBody.AddForce(moveDirection.normalized * runSpeed * 10f, ForceMode.Force);
        
        if(grounded)
            rigidBody.AddForce(moveDirection.normalized * runSpeed * 10f, ForceMode.Force);

        else if(!grounded)
            rigidBody.AddForce(moveDirection.normalized * runSpeed * 10f * airMultiplier, ForceMode.Force);
    }
    
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > runSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * runSpeed;
            rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
        
        rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
    
    
}


