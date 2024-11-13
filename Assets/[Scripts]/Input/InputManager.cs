using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputManager : MonoBehaviour
{
    
    #region Singletone
    private static InputManager Instance;
    public static InputManager GetInstance() 
    { 
        return Instance;
    }
    #endregion
    
    public PlayerControls playerControls { get; private set;}

    [Header("InputActions")] 
    private InputAction moveInput;
    private InputAction cameraLookInput;
    private InputAction jumpInput;
    private InputAction lightAttack;
    private InputAction heavyAttack;
    private InputAction combatMovement;
    private InputAction pauseInput;
    
    [Header("Read values")] 
    private Vector2 vectorMovementValue = default;
    private Vector2 vectorCameraValue = default;
    private bool anyButton = true;

    public static IObservable<InputControl> onAnyButtonPress { get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        playerControls = new PlayerControls();
        playerControls.Enable();
        moveInput = playerControls.PlayerLocomotion.Movement;
        moveInput.Enable();
        cameraLookInput = playerControls.PlayerLocomotion.Look;
        cameraLookInput.Enable();
        jumpInput = playerControls.PlayerLocomotion.Jump;
        jumpInput.Enable();
        pauseInput = playerControls.PlayerLocomotion.Pause;
        pauseInput.Enable();
        lightAttack = playerControls.Combat.LightAttack;
        lightAttack.Disable();
        heavyAttack = playerControls.Combat.HeavyAttack;
        heavyAttack.Disable();
        combatMovement = playerControls.Combat.Movement;
        combatMovement.Disable();
        
    }

    private void FixedUpdate()
    {
        if (GameManager.GetInstance().GetGameState() == GAME_STATE.PRESSANYBUTTON)
        {
            InputSystem.onAnyButtonPress.CallOnce(ctrl => anyButton = ctrl.IsPressed());
        }
        //Debug.Log("any button: " + anyButton);

        if (!anyButton)
        {
            CinemachineSwitcher.GetInstance().SwitchState();
            anyButton = true;
        }
    }

    public Vector2 MovementInput()
    {
        vectorMovementValue = moveInput.ReadValue<Vector2>();
        return vectorMovementValue;
    }

    public Vector2 CameraLookInput()
    {
        vectorCameraValue = cameraLookInput.ReadValue<Vector2>();
        return vectorCameraValue;
    }

    public bool JumpInput()
    {
        return jumpInput.IsInProgress();
    }

    public bool PauseInput()
    {
        return pauseInput.triggered;
    }

    public bool WallJumpInput()
    {
        return jumpInput.triggered;
    }

    public bool LightAttack()
    {
        return lightAttack.triggered;
    }

    public bool HeavyAttack()
    {
        return heavyAttack.triggered;
    }

    public void ActivateCombat()
    {
        playerControls.Combat.Enable();
        playerControls.PlayerLocomotion.Disable();
        playerControls.PlayerLocomotion.Movement.Enable();

    }

    public void DeactivateCombat()
    {
        playerControls.Combat.Disable();
        playerControls.PlayerLocomotion.Enable();
    }

    public void DeactivateInputs()
    {
        playerControls.Combat.Disable();
        playerControls.PlayerLocomotion.Movement.Disable();

    }

   
 
}
