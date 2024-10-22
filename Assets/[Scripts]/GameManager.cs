using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    public static GameManager GetInstance()
    {
        return Instance;
    }

    public GAME_STATE currentGameState;
    public Action<GAME_STATE> OnGameStateChanged;

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
        //currentGameState = GAME_STATE.EXPLORATION;
    }
    
    public void ChangeGameState(GAME_STATE _newState)
    {
        currentGameState = _newState;

        if (OnGameStateChanged != null)
        {
            OnGameStateChanged.Invoke(currentGameState);
        }
    }

    public GAME_STATE GetGameState()
    {
        return currentGameState;
    }
}

public enum GAME_STATE
{
    EXPLORATION,
    PLAYERTURN,
    PLAYERATTACK,
    ENEMYTURN,
    ENEMATACK,
    PRESSANYBUTTON,
    MAINMENU
    
}
