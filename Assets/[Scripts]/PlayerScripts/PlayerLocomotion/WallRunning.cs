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
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpSideForce;
    private float wallRunTimer;

    private float horizontalInput;
    private float verticalInput;
    
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Detection")] 
    [SerializeField] private float wallcheckDisance;
    [SerializeField] private float minJumpHeight;
    [SerializeField] private float playerHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;
    
    public bool useGravity;
    public float gravityCounterForce;
    
    
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
        
        currentPlayerState = _newPlayerStates;
        
        switch (_newPlayerStates)
        {
            case PLAYER_STATES.WALKING:
                //StopWallRunning();
                break;
            case PLAYER_STATES.WALLRUNNING:
                //WallRunningMovement();
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
        Debug.Log("En WallRunning el estado es: " + PlayerStates.GetInstance().GetCurrentPlayerState());
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

                if (InputManager.GetInstance().JumpInput())
                {
                    WallJump();
                }
            }
            else if (exitingWall)
            {
                if (currentPlayerState == PLAYER_STATES.WALLRUNNING)
                    StopWallRunning();

                if (exitWallTimer > 0)
                    exitWallTimer -= Time.deltaTime;

                if (exitWallTimer <= 0)
                    exitingWall = false;
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
        rigidbody.useGravity = useGravity;
       // rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
        
        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;
        
        rigidbody.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rigidbody.AddForce(-wallNormal * 100, ForceMode.Force);
        }
        
        if (useGravity)
            rigidbody.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
       
    }

    private void StopWallRunning()
    {
        Debug.Log("DEJÓ");
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.WALKING);

    }

    void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // reset y velocity and add force
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        rigidbody.AddForce(forceToApply, ForceMode.Impulse);
    }
}
