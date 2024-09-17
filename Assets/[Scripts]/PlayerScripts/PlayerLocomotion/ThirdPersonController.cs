using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Camera playerCamera;
    
    [Header("Movement")]
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private bool readyToJump;
    [SerializeField] private float wallRunSpeed;
    [SerializeField] private float runAcceleration = .25f;
    [SerializeField] private float runSpeed = 4f;
    Vector3 moveDirection;
    float horizontalInput;
    float verticalInput;
    
    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    bool grounded;

    [SerializeField] private Transform orientation;

    private PLAYER_STATES currenPlayerState;
    
    public static Transform targetEnemy;
    public static bool isInCombat = false;
    public GameObject enemyxd;//VARIABLE PARA DEBUG
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
                break;
            case PLAYER_STATES.WALLRUNNING:
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            targetEnemy = other.transform;
            enemyxd = other.gameObject;
            SetTargetGroup.GetInstance().ChangeTargetGroup();
            isInCombat = true;
        }
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        StateHandler();

        if (InputManager.GetInstance().LightAttack())
        {
            Debug.Log(InputManager.GetInstance().LightAttack());
            enemyxd.SetActive(false);
        }
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
            currenPlayerState = PlayerStates.GetInstance().GetCurrentPlayerState();
            runSpeed = wallRunSpeed;
        }
        else if(grounded)
        {
            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.WALKING);
            currenPlayerState = PlayerStates.GetInstance().GetCurrentPlayerState();
            runSpeed = 7;
        }
    }
    

    private void MovePlayer()
    {

        if (targetEnemy != null)
        {
            Vector3 toEnemy = (targetEnemy.position - transform.position).normalized;
            Vector3 enemyRight = Vector3.Cross(Vector3.up, toEnemy);  
            moveDirection = toEnemy * verticalInput + enemyRight * horizontalInput;
        }
        else
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        }

        moveDirection = moveDirection.normalized;
        
        if (grounded)
        {
            rigidBody.AddForce(moveDirection * runSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rigidBody.AddForce(moveDirection * runSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }
    
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

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

    public Transform GetActiveEnemy()
    {
        return targetEnemy;
    }

    public void ResetEnemy()
    {
        targetEnemy = null;
    }
    
    
}


