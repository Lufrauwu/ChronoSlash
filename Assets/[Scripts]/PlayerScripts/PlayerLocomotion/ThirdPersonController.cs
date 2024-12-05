using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Camera playerCamera;
    public bool isInCombat;

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
    [SerializeField] private GameObject modelo;
    public bool happy;

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
    [SerializeField] private GameObject pause;
    public bool paused = false;

    private PLAYER_STATES currenPlayerState;
    
    public static Transform targetEnemy;
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
        Time.timeScale = 1;
        
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
            isInCombat = true;
            targetEnemy = other.transform;
            SetTargetGroup.GetInstance().ChangeTargetGroup();
        }

        if (other.tag == "Exit")
        {
            LevelManager.GetInstance().SwitchScene("Blocking_Exterior");
        }
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        StateHandler();
        Debug.Log("El time es " + Time.timeScale);
        modelo.transform.position = transform.position;
        if (!isInCombat && !paused)
        {
            comboManager2.SetAllToDefault();
            GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
        }

        if (InputManager.GetInstance().PauseInput())
        {

            if (!paused)
            {
                Pause();
                //Time.timeScale = 0;
            }
            else
            {
                //Time.timeScale = 1;
                ResumePause();
                
            }
        }

        
        //Debug.Log("ESTA EN COMBATE " + isInCombat);
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
        
        /*if (!isInCombat)
        {
            InputManager.GetInstance().DeactivateCombat();
        }
        else
        {
            InputManager.GetInstance().ActivateCombat();
        }*/
        
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
        Vector3 moveDir;

        if (targetEnemy != null)
        {
            Vector3 toEnemy = (targetEnemy.position - transform.position).normalized;
            Vector3 enemyRight = Vector3.Cross(Vector3.up, toEnemy);
            moveDir = toEnemy * verticalInput + enemyRight * horizontalInput;
        }
        else
        {
            moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        }

        moveDir = moveDir.normalized;

        // Raycast para ajustar la dirección en pendientes
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, playerHeight * 0.5f + 0.3f, whatIsGround))
        {
            // Ajustar la dirección al plano de la pendiente
            moveDir = Vector3.ProjectOnPlane(moveDir, hit.normal).normalized;
        }

        if (grounded)
        {
            rigidBody.AddForce(moveDir * runSpeed * 10f * Time.fixedDeltaTime, ForceMode.Force);
        }
        else if (!grounded)
        {
            rigidBody.AddForce(moveDir * 7 * 10f * airMultiplier, ForceMode.Force);
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
        if (flatVel.magnitude > runSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * runSpeed;
            rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z);
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

    public void Pause()
    {
        GameManager.GetInstance().ChangeGameState(GAME_STATE.PAUSE);
        paused = true;
        InputManager.GetInstance().SetPause();
        CanvasController.GetInstance().OpenPauseMenu();
        Debug.Log("Game Paused");
        CinemachineSwitcher.GetInstance().PauseCanvas();
    }

    public void ResumePause()
    {
        paused = false;
        Debug.Log("Game Resumed");
        InputManager.GetInstance().DeactivateCombat();
        CinemachineSwitcher.GetInstance().ResumeGame();
        CanvasController.GetInstance().CloseAllMenus();
        GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
    }
    
    
}


