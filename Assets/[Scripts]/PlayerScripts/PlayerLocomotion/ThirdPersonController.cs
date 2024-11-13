using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Camera playerCamera;
    //[SerializeField] private Animator animator;
    
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
    [SerializeField] bool grounded;
    [SerializeField] private Collider _collider;
    [SerializeField] private PhysicMaterial groundedMaterial;

    [SerializeField] private Transform orientation;

    [SerializeField] private ComboManager2 comboManager2;

    private PLAYER_STATES currenPlayerState;
    
    public static Transform targetEnemy;
    public static bool isInCombat = false;
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
            comboManager2.playerTurn = true;
            targetEnemy = other.transform;
            SetTargetGroup.GetInstance().ChangeTargetGroup();
            isInCombat = true;
        }
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        StateHandler();
       /* if (InputManager.GetInstance().PauseInput())
        {
            GameManager.GetInstance().ChangeGameState(GAME_STATE.PAUSE);
        }*/
       /* if (InputManager.GetInstance().LightAttack())
        {
            Debug.Log(InputManager.GetInstance().LightAttack());
            enemyxd.SetActive(false);
        }*/
    }

    private void FixedUpdate()
    {
        Vector3 raycastOrigin = transform.position + Vector3.up * (playerHeight * 0.5f);
        Debug.DrawRay(raycastOrigin, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.red);
        grounded = Physics.Raycast(raycastOrigin, Vector3.down, playerHeight * 0.5f +.2f, whatIsGround);


        if (grounded)
        {
            rigidBody.drag = groundDrag;
            readyToJump = true;
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
            //Invoke(nameof(ResetJump), jumpCooldown);
        }
    }


    private void StateHandler()
    {
        if (wallrunning)
        {
            _collider.material = null;
            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.WALLRUNNING);
            currenPlayerState = PlayerStates.GetInstance().GetCurrentPlayerState();
            runSpeed = wallRunSpeed;
        }
        else if(grounded)
        {
            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.WALKING);
            currenPlayerState = PlayerStates.GetInstance().GetCurrentPlayerState();
            runSpeed = 475;
            
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
            //animator.SetFloat("Velocity", moveDirection.magnitude);
        }

        moveDirection = moveDirection.normalized;
        
        if (grounded)
        {
            _collider.material = null;
            rigidBody.AddForce(moveDirection * runSpeed * 10f * Time.fixedUnscaledDeltaTime, ForceMode.Force );
        }
        else if (!grounded)
        {
           _collider.material = groundedMaterial;
            Vector3 airMoveDirection = moveDirection * 7 * 10f * airMultiplier;
            rigidBody.AddForce(airMoveDirection, ForceMode.Force);
        }
    }
    
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
        if (grounded)
        {
            // Asegúrate de que la velocidad vertical no esté alterando el salto
            rigidBody.velocity = new Vector3(flatVel.x, rigidBody.velocity.y, flatVel.z);
        }
        else
        {
            // Limitar la velocidad horizontal en el aire
            if(flatVel.magnitude > 7)
            { 
                Vector3 limitedVel = flatVel.normalized * 7;
                rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z);
            }
        /*
            // Limitar la velocidad vertical si es necesario
            if (rigidBody.velocity.y > 7.5f)
            {
                Debug.Log("caca");
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, 7.5f, rigidBody.velocity.z);
            }*/
        }
        
    }

    private void Jump()
    {
        //animator.SetBool("Jump", true);
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
        rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        //animator.SetBool("Jump", false);
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


