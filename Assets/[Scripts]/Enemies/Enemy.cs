using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Input = UnityEngine.Windows.Input;

public class Enemy : MonoBehaviour
{
    private ThirdPersonController thirdPersonController;
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damage;
    private HealthController healthController;
    [SerializeField] private ENEMY_TYPE enemyType;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        thirdPersonController = FindAnyObjectByType<ThirdPersonController>();
        switch (enemyType)
        {
            case ENEMY_TYPE.LIGHT:
                maxHealth = 50; 
                damage = 30;
                break;
            case ENEMY_TYPE.MEDIUM:
                maxHealth = 100;
                damage = 20;
                break;
            case ENEMY_TYPE.HEAVY:
                maxHealth = 200;
                damage = 10;
                break;
        }
        
        currentHealth = maxHealth;

    }


    private void OnTriggerEnter(Collider other)
    {
        if(ThirdPersonController.isInCombat)
            return;
        if (other.tag == "Player")
        {
            InputManager.GetInstance().ActivateCombat();
            CameraController.GetInstance().SetLockOn();
            healthController = other.GetComponent<HealthController>();
            GameManager.GetInstance().ChangeGameState(GAME_STATE.PLAYERTURN);
        }
    }

    private void Update()
    {
        if (currentHealth<= 0)
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
        currentHealth -= damage;
    }

    public void Attack()
    {
        Debug.Log("EnemyAttacked");
        animator.SetBool("Attack", true);
        healthController.TakeDamage(damage);
        Debug.Log("YACAMBIO");
        GameManager.GetInstance().ChangeGameState(GAME_STATE.PLAYERTURN);
        //animator.SetBool("Attack", false);

    }
}

public enum ENEMY_TYPE
{
    LIGHT,
    MEDIUM,
    HEAVY
}
