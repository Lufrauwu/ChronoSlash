using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    private static PlayerStates Instance;
    public static PlayerStates GetInstance()
    {
        return Instance;
    }
    
    public PLAYER_STATES currentPlayerState = PLAYER_STATES.WALKING;
    public Action<PLAYER_STATES> OnPlayerStateChanged;
    
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
    }

    private void Start()
    {
        currentPlayerState = PLAYER_STATES.WALKING;
    }
    
    public void ChangePlayerState(PLAYER_STATES _newState)
    {
        currentPlayerState = _newState;

        if (OnPlayerStateChanged != null)
        {
            OnPlayerStateChanged.Invoke(currentPlayerState);
        }
    }
    
    public PLAYER_STATES GetCurrentPlayerState()
    {
        return currentPlayerState;
    }
    
}

public enum PLAYER_STATES
{
    WALKING,
    WALLRUNNING,
}
