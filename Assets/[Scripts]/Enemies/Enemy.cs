using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

public class Enemy : MonoBehaviour
{
    private ThirdPersonController thirdPersonController;
    [SerializeField] private int health = 100;

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
        if (health<= 0)
        {
            this.gameObject.SetActive(false);
        }
        if (GameManager.GetInstance().GetGameState() == GAME_STATE.ENEMYTURN)
        {
            Attack();
        }
    }

    private void OnDisable()
    {
        InputManager.GetInstance().DeactivateCombat();
        CameraController.GetInstance().SetLockOff();
        thirdPersonController.ResetEnemy();
        ThirdPersonController.isInCombat = false;
        GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Attack()
    {
        Debug.Log("EnemyAttacked");
        GameManager.GetInstance().ChangeGameState(GAME_STATE.PLAYERTURN);
    }
}
