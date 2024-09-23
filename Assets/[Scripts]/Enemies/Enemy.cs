using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

public class Enemy : MonoBehaviour
{
    private ThirdPersonController tpController;

    private void Awake()
    {
        tpController = FindAnyObjectByType<ThirdPersonController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(ThirdPersonController.isInCombat)
            return;
        if (other.tag == "Player")
        {
            InputManager.GetInstance().ActivateCombat();
            CameraController.GetInstance().SetLockOn();
        }
    }

    private void OnDisable()
    {
        InputManager.GetInstance().DeactivateCombat();
        CameraController.GetInstance().SetLockOff();
        tpController.ResetEnemy();
        ThirdPersonController.isInCombat = false;
    }
}
