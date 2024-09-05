using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{

    [Header("Wall Running")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float wallRunForce;
    [SerializeField] private float maxWallRunTime;
    private float wallRunTimer;

    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")] 
    [SerializeField] private float wallcheckDisance;
    [SerializeField] private float minJumpHeight;
    [SerializeField] private float playerHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;
    
    
    [Header("References")]
    [SerializeField] private Transform orientation;
    private Rigidbody rigidbody;
    private ThirdPersonController thirdPersonController;
    private PLAYER_STATES currentPlayerState;
    
    
    private void SubscribeToPlayerState()
    {
        PlayerStates.GetInstance().OnPlayerStateChanged += PlayerStateChange;
        PlayerStateChange(PlayerStates.GetInstance().GetCurrentPlayerState());
    }
    
    private void PlayerStateChange(PLAYER_STATES _newPlayerStates)
    {
        switch (_newPlayerStates)
        {
            case PLAYER_STATES.WALKING:
                StopWallRunning();
                break;
            case PLAYER_STATES.WALLRUNNING:
                WallRunningMovement();
                break;
        }
    }
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        currentPlayerState = PlayerStates.GetInstance().GetCurrentPlayerState();
        SubscribeToPlayerState();

    }


    void Update()
    {
        CheckWall();
        StateMachine();
        Debug.Log(AboceGround());
    }

    private void FixedUpdate()
    {
        if (currentPlayerState == PLAYER_STATES.WALLRUNNING)
        {
            WallRunningMovement();
        }
    }

    private void CheckWall()
    {
        Vector3 raycastOrigin = transform.position + Vector3.up * (playerHeight * 0.5f);
        wallRight = Physics.Raycast(raycastOrigin, orientation.right, out rightWallHit, wallcheckDisance, wallLayer);
        Debug.DrawRay(raycastOrigin, orientation.right * wallcheckDisance, Color.red);
        wallLeft = Physics.Raycast(raycastOrigin, -orientation.right, out leftWallHit, wallcheckDisance, wallLayer);
        Debug.DrawRay(raycastOrigin, -orientation.right * wallcheckDisance, Color.blue);
    }

    private bool AboceGround()
    {
        Vector3 raycastOrigin = transform.position + Vector3.up * (playerHeight * 0.5f);
        return !Physics.Raycast(raycastOrigin, Vector3.down, minJumpHeight, groundLayer);
    }

    private void StateMachine()
    {
        horizontalInput = InputManager.GetInstance().MovementInput().x;
        verticalInput = InputManager.GetInstance().MovementInput().y;

        if ((wallLeft || wallRight) && verticalInput > 0 && AboceGround())
        {
            if (currentPlayerState != PLAYER_STATES.WALLRUNNING)
            {
                StartWallRunning();
            }
            else
            {
                if (currentPlayerState == PLAYER_STATES.WALLRUNNING)
                {
                    StopWallRunning();
                }
            }
        }
    }

    private void StartWallRunning()
    {
        Debug.Log("StartWallRunning");
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.WALLRUNNING);
        currentPlayerState = PlayerStates.GetInstance().GetCurrentPlayerState();
    }

    private void WallRunningMovement()
    {
        Debug.Log("WALLRUNNING");
        rigidbody.useGravity = false;
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
        rigidbody.AddForce(wallForward * wallRunForce, ForceMode.Force);
    }

    private void StopWallRunning()
    {
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.WALKING);

    }
}
