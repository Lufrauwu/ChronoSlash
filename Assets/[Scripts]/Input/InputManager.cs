using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    
    [Header("Read values")] 
    private Vector2 vectorMovementValue = default;
    private Vector2 vectorCameraValue = default;


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

   
 
}
