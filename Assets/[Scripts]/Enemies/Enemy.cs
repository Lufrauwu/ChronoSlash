using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

public class Enemy : MonoBehaviour
{
    private ThirdPersonController thirdPersonController;

    private void Awake()
    {
        thirdPersonController = FindAnyObjectByType<ThirdPersonController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(ThirdPersonController.isInCombat)
            return;
        if (other.tag == "Player")
        {
            InputManager.GetInstance().ActivateCombat();
            CameraController.GetInstance().SetLockOn();
            GameManager.GetInstance().ChangeGameState(GAME_STATE.PLAYERTURN);
        }
    }

    private void Update()
    {
        if (GameManager.GetInstance().GetGameState() == GAME_STATE.ENEMYTURN)
        {
            
        }
    }

    private void OnDisable()
    {
        InputManager.GetInstance().DeactivateCombat();
        CameraController.GetInstance().SetLockOff();
        thirdPersonController.ResetEnemy();
        ThirdPersonController.isInCombat = false;
    }
}
